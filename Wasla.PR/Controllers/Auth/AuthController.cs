using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wasla.DAL.Identity;
using Wasla.Enums;
using Wasla.PR.ViewModels.Auth;
using Wasla.Models;

namespace Wasla.PR.Controllers.Auth
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.UserType == UserType.Merchant)
            {
                return View("RegisterMerchant", new RegisterMerchantViewModel
                {
                    Name = model.Name,
                    Phone = model.Phone,
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                });
            }

            return View("RegisterCompany", new RegisteCompanyViewModel
            {
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword
            });
        }

        public IActionResult RegisterMerchant()
        {
            return View();
        }

        public IActionResult RegisterCompany()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RegisterMerchant(RegisterMerchantViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone,
                UserType = UserType.Merchant
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            _context.Merchants.Add(new Wasla.Models.Merchant
            {
                Name = model.Name,
                StoreName = model.StoreName,
                TaxNumber = model.TaxNumber,
                Category = model.Category,
                UserId = user.Id
            });

            await _context.SaveChangesAsync();

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Merchant");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCompany(RegisteCompanyViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone,
                UserType = UserType.Company
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            _context.CourierCompanies.Add(new CourierCompany
            {
                Name = model.Name,
                CompanyName = model.CompanyName,
                TaxNumber = model.TaxNumber,
                UserId = user.Id
            });

            await _context.SaveChangesAsync();

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Company");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                false,   // RememberMe
                false);  // Lockout

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }
            if (user.UserType == UserType.Admin)
                return RedirectToAction("Dashboard", "Admin");

            if (user.UserType == UserType.Merchant)
                return RedirectToAction("Index", "Merchant");

            if (user.UserType == UserType.Company)
                return RedirectToAction("Index", "Company");
            if (user.UserType == UserType.Driver)
                return RedirectToAction("Index", "Agent");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Auth");
        }
    }
}
