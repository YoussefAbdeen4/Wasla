using Microsoft.EntityFrameworkCore;
using Wasla.Models;

namespace Wasla.BLL.Services.Admin
{
    public class AdminDriversService
    {
        private readonly AppDbContext _context;

        public AdminDriversService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Driver>> GetAllDriversAsync()
        {
            return await _context.Drivers
                .Include(d => d.User)
                .Include(d => d.Phones)
                .Include(d => d.Company)
                .Include(d => d.Orders)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<Driver?> GetDriverDetailsAsync(int id)
        {
            return await _context.Drivers
                .Include(d => d.User)
                .Include(d => d.Phones)
                .Include(d => d.Company)
                .Include(d => d.Orders)
                .Include(d => d.DriverVehicles)
                    .ThenInclude(dv => dv.Vehicle)
                .FirstOrDefaultAsync(d => d.Id == id);
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