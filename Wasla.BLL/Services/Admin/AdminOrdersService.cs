using Microsoft.EntityFrameworkCore;
using Wasla.Models;

namespace Wasla.BLL.Services
{
    public class AdminOrdersService
    {
        private readonly AppDbContext _context;

        public AdminOrdersService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all orders in the system (used in Admin Orders list screen)
        /// </summary>
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Get recent orders (used in Admin Dashboard screen)
        /// </summary>
        public async Task<List<Order>> GetRecentOrdersAsync(int count = 10)
        {
            return await _context.Orders
                .OrderByDescending(o => o.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// Get full order details including merchant, company, driver and products (used in Admin Order Details screen)
        /// </summary>
        public async Task<Order?> GetOrderDetailsAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Merchant)
                .Include(o => o.Company)
                .Include(o => o.Driver)
                .Include(o => o.OrderProducts)
                .Include(o => o.TrackingHistories)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}