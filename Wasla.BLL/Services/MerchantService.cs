using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wasla.Models;

namespace Wasla.BLL.Services
{
    public class MerchantDashboardDto
    {
        public int TotalOrdersCount { get; set; }
        public int DeliveredOrdersCount { get; set; }
        public int RejectedOrdersCount { get; set; }
        public int UnderReviewOrdersCount { get; set; }
        public decimal TotalSales { get; set; }
        public List<Order> RecentOrders { get; set; } = new List<Order>();
    }

    public class MerchantService
    {
        private readonly AppDbContext _context;

        public MerchantService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Merchant?> GetMerchantByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            return await _context.Merchants
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UserId == userId);
        }

        public async Task<Order?> GetOrderDetailsAsync(int orderId, string userId)
        {
            var merchant = await _context.Merchants.FirstOrDefaultAsync(m => m.UserId == userId);
            if (merchant == null)
                return null;

            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .Include(o => o.TrackingHistories)
                .Include(o => o.Driver)
                .Include(o => o.Merchant)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.MerchantId == merchant.Id);

            return order;
        }

        public async Task<Order> CreateOrderAsync(int merchantId, Order newOrder)
        {
            newOrder.MerchantId = merchantId;
            newOrder.CreatedAt = DateTime.Now;
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();
            return newOrder;
        }

        public async Task<Merchant?> UpdateMerchantProfileAsync(string userId, Merchant updated)
        {
            var merchant = await _context.Merchants.FirstOrDefaultAsync(m => m.UserId == userId);
            if (merchant == null)
                return null;

            merchant.Name = updated.Name;
            merchant.StoreName = updated.StoreName;
            merchant.TaxNumber = updated.TaxNumber;
            merchant.Category = updated.Category;

            await _context.SaveChangesAsync();
            return merchant;
        }

        public async Task<decimal> GetWalletBalanceAsync(int merchantId)
        {
            var m = await _context.Merchants.AsNoTracking().FirstOrDefaultAsync(x => x.Id == merchantId);
            return m?.WalletBalance ?? 0m;
        }

        public async Task<MerchantDashboardDto> GetDashboardAsync(string userId, int recentCount = 5)
        {
            var dto = new MerchantDashboardDto();

            var merchant = await _context.Merchants
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (merchant == null)
                return dto; // empty

            var ordersQuery = _context.Orders
                .AsNoTracking()
                .Where(o => o.MerchantId == merchant.Id);

            dto.TotalOrdersCount = await ordersQuery.CountAsync();
            dto.DeliveredOrdersCount = await ordersQuery.CountAsync(o => o.status == Enums.OrderStatus.Delivered);
            dto.RejectedOrdersCount = await ordersQuery.CountAsync(o => o.status == Enums.OrderStatus.Cancelled || o.status == Enums.OrderStatus.FailedDelivery);
            dto.UnderReviewOrdersCount = await ordersQuery.CountAsync(o => o.status == Enums.OrderStatus.PendingConfirmation || o.status == Enums.OrderStatus.Packed || o.status == Enums.OrderStatus.OutForDelivery);

            dto.TotalSales = await ordersQuery.SumAsync(o => (decimal?)o.TotalPrice) ?? 0m;

            dto.RecentOrders = await ordersQuery
                .OrderByDescending(o => o.CreatedAt)
                .Take(recentCount)
                .ToListAsync();

            return dto;
        }

        public async Task<List<Order>> GetRecentOrdersAsync(int merchantId, int take = 10)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.MerchantId == merchantId)
                .OrderByDescending(o => o.CreatedAt)
                .Take(take)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync(int merchantId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.MerchantId == merchantId && o.status == Enums.OrderStatus.Delivered)
                .SumAsync(o => (decimal?)o.TotalPrice) ?? 0m;
        }
    }
}
