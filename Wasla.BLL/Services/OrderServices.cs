using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Wasla.DAL.Identity;
using Wasla.Enums;
using Wasla.Models;

namespace Wasla.BLL.Services
{

    public class OrderServices
    {

        private readonly AppDbContext _context;

        public OrderServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetCompanyOrdersAsync(string companyUserId)
        {
            var company = await _context.CourierCompanies
                .FirstOrDefaultAsync(c => c.UserId == companyUserId);

            if (company == null)
                throw new Exception("Company not found");

            return await _context.Orders
                .Where(o => o.CompanyId == company.Id)
                .Include(o => o.Driver)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Get full order details including products, tracking history, and financial info
        /// </summary>
        public async Task<Order> GetOrderDetailsAsync(int orderId, int companyId)
        {
            var order = await _context.Orders
                .Where(o => o.Id == orderId && o.CompanyId == companyId)
                .Include(o => o.Driver)
                .Include(o => o.OrderProducts)
                .Include(o => o.TrackingHistories)
                .Include(o => o.Merchant)
                .FirstOrDefaultAsync();

            if (order == null)
                throw new Exception("الطلب غير موجود أو أنت غير مصرح بالوصول إليه");

            return order;
        }

        /// <summary>
        /// Change order status with validation
        /// </summary>
        public async Task ChangeOrderStatusAsync(int orderId, int companyId, OrderStatus newStatus)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.CompanyId == companyId);

            if (order == null)
                throw new Exception("الطلب غير موجود أو أنت غير مصرح بالوصول إليه");

            // Validate status transition
            if (!IsValidStatusTransition(order.status, newStatus))
                throw new Exception("انتقال الحالة هذا غير مسموح");

            order.status = newStatus;
            order.UpdatedAt = DateTime.Now;

            if (newStatus == OrderStatus.Delivered)
                order.DeliveredAt = DateTime.Now;

            // Add tracking history
            var trackingHistory = new TrackingHistory
            {
                OrderId = orderId,
                Status = newStatus,
                Timestamp = DateTime.Now,
                Location = "تحديث الحالة"
            };

            _context.TrackingHistories.Add(trackingHistory);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Assign a driver to an order
        /// </summary>
        public async Task AssignDriverToOrderAsync(int orderId, int companyId, int driverId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.CompanyId == companyId);

            if (order == null)
                throw new Exception("الطلب غير موجود أو أنت غير مصرح بالوصول إليه");

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.Id == driverId && d.CompanyId == companyId);

            if (driver == null)
                throw new Exception("السائق غير موجود أو لا ينتمي لشركتك");

            order.DriverId = driverId;
            order.UpdatedAt = DateTime.Now;

            // Add tracking history
            var trackingHistory = new TrackingHistory
            {
                OrderId = orderId,
                Status = order.status,
                Timestamp = DateTime.Now,
                Location = $"إسناد الطلب للسائق {driver.Name}"
            };

            _context.TrackingHistories.Add(trackingHistory);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get list of available drivers for a company
        /// </summary>
        public async Task<List<Driver>> GetCompanyDriversAsync(int companyId)
        {
            return await _context.Drivers
                .Where(d => d.CompanyId == companyId)
                .Include(d => d.User)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Validate if status transition is allowed
        /// </summary>
        private bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            // Define valid transitions
            var validTransitions = new Dictionary<OrderStatus, List<OrderStatus>>
            {
                { OrderStatus.Created, new List<OrderStatus> { OrderStatus.PendingConfirmation, OrderStatus.Cancelled } },
                { OrderStatus.PendingConfirmation, new List<OrderStatus> { OrderStatus.Packed, OrderStatus.Cancelled } },
                { OrderStatus.Packed, new List<OrderStatus> { OrderStatus.OutForDelivery, OrderStatus.Cancelled } },
                { OrderStatus.OutForDelivery, new List<OrderStatus> { OrderStatus.Delivered, OrderStatus.FailedDelivery } },
                { OrderStatus.Delivered, new List<OrderStatus> { } },
                { OrderStatus.FailedDelivery, new List<OrderStatus> { OrderStatus.OutForDelivery, OrderStatus.Returned } },
                { OrderStatus.Returned, new List<OrderStatus> { } },
                { OrderStatus.Cancelled, new List<OrderStatus> { } }
            };

            if (!validTransitions.ContainsKey(currentStatus))
                return false;

            return validTransitions[currentStatus].Contains(newStatus);
        }
    }
}

