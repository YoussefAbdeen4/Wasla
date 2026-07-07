using Microsoft.EntityFrameworkCore;
using Wasla.Enums;
using Wasla.Models;

namespace Wasla.BLL.Services
{
    public class CompanyDashboardService
    {
        private readonly AppDbContext _context;

        public CompanyDashboardService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get dashboard statistics and recent data for company
        /// </summary>
        public async Task<(DashboardStatistics stats, List<RecentOrder> recentOrders, List<AvailableDriver> drivers)> GetDashboardDataAsync(int companyId)
        {
            try
            {
                // Get all company orders
                var orders = await _context.Orders
                    .Where(o => o.CompanyId == companyId)
                    .Include(o => o.Driver)
                    .ToListAsync();

                // Calculate statistics
                var stats = new DashboardStatistics
                {
                    TotalOrders = orders.Count,
                    PendingOrders = orders.Count(o => o.status == OrderStatus.PendingConfirmation || o.status == OrderStatus.Created),
                    OutForDeliveryOrders = orders.Count(o => o.status == OrderStatus.OutForDelivery),
                    DeliveredOrders = orders.Count(o => o.status == OrderStatus.Delivered),
                    FailedOrders = orders.Count(o => o.status == OrderStatus.FailedDelivery || o.status == OrderStatus.Returned),
                    TotalRevenue = orders.Sum(o => o.TotalPrice),
                    TotalDrivers = await _context.Drivers.CountAsync(d => d.CompanyId == companyId),
                    TotalAgents = await _context.Drivers.CountAsync(d => d.CompanyId == companyId)
                };

                // Get recent orders (last 10)
                var recentOrders = orders
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(10)
                    .Select(o => new RecentOrder
                    {
                        Id = o.Id,
                        TrackingUuid = o.TrackingUuid,
                        CustomerName = o.CustomerName,
                        DriverName = o.Driver?.Name ?? "غير مسند",
                        TotalPrice = o.TotalPrice,
                        Status = o.status,
                        CreatedAt = o.CreatedAt
                    })
                    .ToList();

                // Get available drivers
                var drivers = await _context.Drivers
                    .Where(d => d.CompanyId == companyId)
                    .Include(d => d.User)
                    .Include(d => d.DriverOrders)
                    .Select(d => new AvailableDriver
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Phone = d.User.PhoneNumber,
                        ActiveOrders = d.DriverOrders.Count(do_ => do_.Order.status != OrderStatus.Delivered && do_.Order.status != OrderStatus.Cancelled)
                    })
                    .ToListAsync();

                return (stats, recentOrders, drivers);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في جلب بيانات لوحة التحكم: {ex.Message}");
            }
        }
    }

    public class DashboardStatistics
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int OutForDeliveryOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int FailedOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalDrivers { get; set; }
        public int TotalAgents { get; set; }
    }

    public class RecentOrder
    {
        public int Id { get; set; }
        public string TrackingUuid { get; set; }
        public string CustomerName { get; set; }
        public string DriverName { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AvailableDriver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int ActiveOrders { get; set; }
    }
}
