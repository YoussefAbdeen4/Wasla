using Microsoft.EntityFrameworkCore;
using Wasla.Models;

namespace Wasla.BLL.Services
{
    public class CompanyService
    {
        private readonly AppDbContext _context;

        public CompanyService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get company by user ID
        /// </summary>
        public async Task<CourierCompany> GetCompanyByUserIdAsync(string userId)
        {
            var company = await _context.CourierCompanies
                .Include(c => c.User)
                .Include(c => c.Phones)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
                throw new Exception("الشركة غير موجودة");

            return company;
        }

        public async Task<List<RateCard>> GetRateCardsByCompanyIdAsync(int companyId)
        {
            return await _context.RateCards
                .Where(r => r.CompanyId == companyId)
                .ToListAsync();
        }

        /// <summary>
        /// Get company by ID
        /// </summary>
        public async Task<CourierCompany> GetCompanyByIdAsync(int companyId)
        {
            var company = await _context.CourierCompanies
                .Include(c => c.User)
                .Include(c => c.Phones)
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new Exception("الشركة غير موجودة");

            return company;
        }

        /// <summary>
        /// Update company profile information
        /// </summary>
        public async Task UpdateCompanyProfileAsync(CourierCompany company)
        {
            try
            {
                _context.CourierCompanies.Update(company);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء تحديث بيانات الشركة: {ex.Message}");
            }
        }

        /// <summary>
        /// Get company statistics
        /// </summary>
        public async Task<CompanyStatistics> GetCompanyStatisticsAsync(int companyId)
        {
            var drivers = await _context.Drivers.CountAsync(d => d.CompanyId == companyId);
            var vehicles = await _context.Vehicles.CountAsync(v => v.CompanyId == companyId);
            var totalOrders = await _context.Orders.CountAsync(o => o.CompanyId == companyId);
            var rateCards = await _context.RateCards.CountAsync(r => r.CompanyId == companyId);

            return new CompanyStatistics
            {
                TotalDrivers = drivers,
                TotalVehicles = vehicles,
                TotalOrders = totalOrders,
                TotalRateCards = rateCards
            };
        }
    }

    public class CompanyStatistics
    {
        public int TotalDrivers { get; set; }
        public int TotalVehicles { get; set; }
        public int TotalOrders { get; set; }
        public int TotalRateCards { get; set; }
    }
}
