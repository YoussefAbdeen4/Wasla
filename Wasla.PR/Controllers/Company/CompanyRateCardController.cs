using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Wasla.BLL.Services;
using Wasla.DAL.Identity;
using Wasla.Enums;
using Wasla.Models;
using Wasla.PR.ViewModels.RateCards;

namespace Wasla.PR.Controllers.Company
{
    [Authorize]
    public class CompanyRateCardController : Controller
    {
        private readonly RateCardService _rateCardService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public CompanyRateCardController(
            RateCardService rateCardService,
            UserManager<ApplicationUser> userManager,
            AppDbContext context)
        {
            _rateCardService = rateCardService;
            _userManager = userManager;
            _context = context;
        }

        private async Task<CourierCompany> GetUserCompanyAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.CourierCompanies
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var company = await GetUserCompanyAsync();
            if (company == null)
                return Unauthorized();

            var rateCards = await _rateCardService.GetAllAsync(company.Id);
            var viewModel = rateCards.Select(rc => new RateCardListViewModel
            {
                Id = rc.Id,
                OriginCity = rc.OriginCity.ToString(),
                DestinationCity = rc.DestinationCity.ToString(),
                MinWeight = rc.MinWeight,
                MaxWeight = rc.MaxWeight,
                BaseFee = rc.BaseFee,
                ExtraKiloPrice = rc.ExtraKiloPrice,
                EffectiveDate = rc.EffectiveDate,
                ExpiryDate = rc.ExpiryDate
            }).ToList();

            return View("~/Views/Company/RateCards/Index.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var company = await GetUserCompanyAsync();
            if (company == null)
                return Unauthorized();

            var rateCard = await _rateCardService.GetByIdAsync(id, company.Id);
            if (rateCard == null)
                return NotFound();

            var viewModel = new RateCardDetailsViewModel
            {
                Id = rateCard.Id,
                OriginCity = rateCard.OriginCity.ToString(),
                DestinationCity = rateCard.DestinationCity.ToString(),
                MinWeight = rateCard.MinWeight,
                MaxWeight = rateCard.MaxWeight,
                BaseFee = rateCard.BaseFee,
                ExtraKiloPrice = rateCard.ExtraKiloPrice,
                EffectiveDate = rateCard.EffectiveDate,
                ExpiryDate = rateCard.ExpiryDate
            };

            return View("~/Views/Company/RateCards/Details.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var company = await GetUserCompanyAsync();
            if (company == null)
                return Unauthorized();

            return View("~/Views/Company/RateCards/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRateCardViewModel model)
        {
            var company = await GetUserCompanyAsync();
            if (company == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return View("~/Views/Company/RateCards/Create.cshtml", model);

            if (model.MinWeight >= model.MaxWeight)
            {
                ModelState.AddModelError("", "Minimum Weight must be less than Maximum Weight");
                return View("~/Views/Company/RateCards/Create.cshtml", model);
            }

            if (model.EffectiveDate >= model.ExpiryDate)
            {
                ModelState.AddModelError("", "Effective Date must be before Expiry Date");
                return View("~/Views/Company/RateCards/Create.cshtml", model);
            }

            var rateCard = new RateCard
            {
                OriginCity = model.OriginCity,
                DestinationCity = model.DestinationCity,
                MinWeight = model.MinWeight,
                MaxWeight = model.MaxWeight,
                BaseFee = model.BaseFee,
                ExtraKiloPrice = model.ExtraKiloPrice,
                EffectiveDate = model.EffectiveDate,
                ExpiryDate = model.ExpiryDate,
                CompanyId = company.Id
            };

            await _rateCardService.CreateAsync(rateCard);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var company = await GetUserCompanyAsync();
            if (company == null)
                return Unauthorized();

            var rateCard = await _rateCardService.GetByIdAsync(id, company.Id);
            if (rateCard == null)
                return NotFound();

            var viewModel = new EditRateCardViewModel
            {
                Id = rateCard.Id,
                OriginCity = rateCard.OriginCity,
                DestinationCity = rateCard.DestinationCity,
                MinWeight = rateCard.MinWeight,
                MaxWeight = rateCard.MaxWeight,
                BaseFee = rateCard.BaseFee,
                ExtraKiloPrice = rateCard.ExtraKiloPrice,
                EffectiveDate = rateCard.EffectiveDate,
                ExpiryDate = rateCard.ExpiryDate
            };

            return View("~/Views/Company/RateCards/Edit.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditRateCardViewModel model)
        {
            var company = await GetUserCompanyAsync();
            if (company == null)
                return Unauthorized();

            if (id != model.Id)
                return BadRequest();

            var rateCard = await _rateCardService.GetByIdAsync(id, company.Id);
            if (rateCard == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View("~/Views/Company/RateCards/Edit.cshtml", model);

            if (model.MinWeight >= model.MaxWeight)
            {
                ModelState.AddModelError("", "Minimum Weight must be less than Maximum Weight");
                return View("~/Views/Company/RateCards/Edit.cshtml", model);
            }

            if (model.EffectiveDate >= model.ExpiryDate)
            {
                ModelState.AddModelError("", "Effective Date must be before Expiry Date");
                return View("~/Views/Company/RateCards/Edit.cshtml", model);
            }

            rateCard.OriginCity = model.OriginCity;
            rateCard.DestinationCity = model.DestinationCity;
            rateCard.MinWeight = model.MinWeight;
            rateCard.MaxWeight = model.MaxWeight;
            rateCard.BaseFee = model.BaseFee;
            rateCard.ExtraKiloPrice = model.ExtraKiloPrice;
            rateCard.EffectiveDate = model.EffectiveDate;
            rateCard.ExpiryDate = model.ExpiryDate;

            await _rateCardService.UpdateAsync(rateCard);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await GetUserCompanyAsync();
            if (company == null)
                return Unauthorized();

            var result = await _rateCardService.DeleteAsync(id, company.Id);
            if (!result)
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}
