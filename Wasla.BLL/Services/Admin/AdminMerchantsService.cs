using Microsoft.EntityFrameworkCore;
using Wasla.Models;

namespace Wasla.BLL.Services
{
    public class AdminMerchantsService
    {
        private readonly AppDbContext _context;

        public AdminMerchantsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Merchant>> GetAllMerchantsAsync()
        {
            return await _context.Merchants
                .Include(m => m.User)
                .Include(m => m.Phones)
                .Include(m => m.Orders)
                .OrderBy(m => m.StoreName)
                .ToListAsync();
        }

        public async Task<Merchant?> GetMerchantDetailsAsync(int id)
        {
            return await _context.Merchants
                .Include(m => m.User)
                .Include(m => m.Phones)
                .Include(m => m.Orders)
                .Include(m => m.Ratings)
                .Include(m => m.Clients)
                .FirstOrDefaultAsync(m => m.Id == id);
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
    }
}