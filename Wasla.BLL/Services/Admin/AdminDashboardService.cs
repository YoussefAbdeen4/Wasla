using Microsoft.EntityFrameworkCore;
using Wasla.Models;

namespace Wasla.BLL.Services.Admin
{
    public class AdminDashboardService
    {
        private readonly AppDbContext _context;

        public AdminDashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(int totalCompanies, int totalMerchants, int totalOrders, int totalDrivers)> GetGlobalStatisticsAsync()
        {
            var totalCompanies = await _context.CourierCompanies.CountAsync();
            var totalMerchants = await _context.Merchants.CountAsync();
            var totalOrders = await _context.Orders.CountAsync();
            var totalDrivers = await _context.Drivers.CountAsync();

            return (totalCompanies, totalMerchants, totalOrders, totalDrivers);
        }

        public async Task<List<Order>> GetRecentOrdersAsync(int count = 10)
        {
            return await _context.Orders
                .OrderByDescending(o => o.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<CourierCompany>> GetRecentCompaniesAsync(int count = 10)
        {
            return await _context.CourierCompanies
                .Include(c => c.User)
                .Include(c => c.Phones)
                .Include(c => c.Orders)
                .OrderByDescending(c => c.Id)
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// Derive account status from Identity's LockoutEnd since no explicit Status field exists yet
        /// </summary>
        public string GetAccountStatus(DateTimeOffset? lockoutEnd)
        {
            if (lockoutEnd == null || lockoutEnd <= DateTimeOffset.Now)
                return "Active";

            return "Suspended";
        }

        public async Task<List<CourierCompany>> GetAllCompaniesAsync()
        {
            return await _context.CourierCompanies
                .Include(c => c.User)
                .Include(c => c.Phones)
                .Include(c => c.Orders)
                .OrderBy(c => c.CompanyName)
                .ToListAsync();
        }
        public async Task<List<Merchant>> GetAllMerchantsAsync()
        {
            return await _context.Merchants
                .Include(m => m.User)
                .Include(m => m.Phones)
                .Include(m => m.Orders)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }
    }
}