using Microsoft.EntityFrameworkCore;
using Wasla.DAL.Identity;
using Wasla.Models;

namespace Wasla.BLL.Services
{
    public class RateCardService
    {
        private readonly AppDbContext _context;

        public RateCardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RateCard>> GetAllAsync(int companyId)
        {
            return await _context.RateCards
                .Where(r => r.CompanyId == companyId)
                .OrderBy(r => r.EffectiveDate)
                .ToListAsync();
        }

        public async Task<RateCard?> GetByIdAsync(int id, int companyId)
        {
            return await _context.RateCards
                .FirstOrDefaultAsync(r => r.Id == id && r.CompanyId == companyId);
        }

        public async Task CreateAsync(RateCard rateCard)
        {
            _context.RateCards.Add(rateCard);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RateCard rateCard)
        {
            _context.RateCards.Update(rateCard);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id, int companyId)
        {
            var rateCard = await GetByIdAsync(id, companyId);
            if (rateCard == null)
                return false;

            _context.RateCards.Remove(rateCard);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
