using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Wasla.BLL.Services;
using Wasla.DAL.Identity;
using Wasla.Enums;
using Wasla.Models;
using Wasla.PR.ViewModels.Agent;

namespace Wasla.PR.Controllers.Company
{
    [Authorize]
    public class CompanyAgentController : Controller
    {
        private readonly AgentService _agentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public CompanyAgentController(AgentService agentService, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _agentService = agentService;
            _userManager = userManager;
            _context = context;
        }
        // GET: AgentController
        public async Task<IActionResult> Index()
        {
            var companyUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var company = await _context.CourierCompanies
                .FirstOrDefaultAsync(c => c.UserId == companyUserId);

            if (company == null)
                return Unauthorized();

            var drivers = await _agentService.GetAgentsAsync(company.Id);

            var model = drivers.Select(d => new AgentListItemViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Phone = d.User?.PhoneNumber,
                Email = d.User?.Email,
                Status = "active",
                Avatar = null,
                OrdersCount = d.DriverOrders?.Count ?? 0
            }).ToList();

            return View("~/Views/Company/Agents/Index.cshtml", model);
        }

        // GET: AgentController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var company = await _context.CourierCompanies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
                return Unauthorized();

            var driver = await _agentService.GetByIdAsync(id, company.Id);

            if (driver == null)
                return NotFound();

            var vehicle = driver.DriverVehicles.FirstOrDefault()?.Vehicle;

            var model = new AgentDetailsViewModel
            {
                Id = driver.Id,
                Name = driver.Name,
                Phone = driver.User?.PhoneNumber,

                VehicleType = vehicle?.Type.ToString() ?? "غير محدد",

                PlateNumber = vehicle.LicensePlate,

                Status = "active",

                Avatar = null,
                PendingOrders = driver.DriverOrders?
                        .Count(o =>
                            o.Order.status == OrderStatus.Created ||
                            o.Order.status == OrderStatus.PendingConfirmation ||
                            o.Order.status == OrderStatus.Packed
                        ) ?? 0,

                                    CompletedOrders = driver.DriverOrders?
                        .Count(o => o.Order.status == OrderStatus.Delivered) ?? 0,  
                                    ReturnedOrders = driver.DriverOrders?
                        .Count(o => o.Order.status == OrderStatus.Returned ||
                                    o.Order.status == OrderStatus.FailedDelivery) ?? 0
            };

            return View("~/Views/Company/Agents/Details.cshtml", model);
        }

        // GET: AgentController/Create
        public ActionResult Create()
        {
            return View("~/Views/Company/Agents/Add.cshtml");
        }

        // POST: AgentController/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateDriverViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Company/Agents/Add.cshtml", model);

            try
            {
                var user = await _userManager.GetUserAsync(User);

                var driver = new Driver
                {
                    Name = model.Name,
                    User = new ApplicationUser
                    {
                        Email = model.Email,
                        UserName = model.Email,
                        PhoneNumber = model.Phone
                    }
                };

                var vehicle = new Vehicle
                {
                    LicensePlate = model.LicensePlate,
                    Type = model.Vehicle,
                    Capacity = 0,
                    IsActive = IsActiveStatus.Active,
                };

                await _agentService.CreateAsync(driver, vehicle, model.Password, user.Id);

                TempData["Success"] = "Driver created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("~/Views/Company/Agents/Add.cshtml", model);
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            var companyUserId = _userManager.GetUserId(User);

            var company = await _context.CourierCompanies
                .FirstOrDefaultAsync(c => c.UserId == companyUserId);

            if (company == null)
                return NotFound();

            var driver = await _agentService.GetByIdAsync(id, company.Id);

            if (driver == null)
                return NotFound();

            var vehicle = driver.DriverVehicles.FirstOrDefault()?.Vehicle;

            var model = new EditDriverViewModel
            {
                Id = driver.Id,
                Name = driver.Name,
                Email = driver.User.Email,
                Phone = driver.User?.PhoneNumber,
                Vehicle = vehicle.Type,
                LicensePlate = vehicle.LicensePlate
            };

            return View("~/Views/Company/Agents/Edit.cshtml", model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditDriverViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var companyUserId = _userManager.GetUserId(User);

            var driver = new Driver
            {
                Id = vm.Id,
                Name = vm.Name,
                User = new ApplicationUser
                {
                    Email = vm.Email,
                    UserName = vm.Email,
                    PhoneNumber = vm.Phone
                },
                DriverVehicles = new List<DriverVehicle>
                {
                    new DriverVehicle
                    {
                        Vehicle = new Vehicle
                        {
                            Type = vm.Vehicle,
                            LicensePlate = vm.LicensePlate
                        }
                    }
                }
            };

            await _agentService.UpdateAsync(driver, companyUserId);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var company = await _context.CourierCompanies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
                return Unauthorized();

            try
            {
                var result = await _agentService.DeleteAsync(id, company.Id);

                if (!result)
                    return NotFound();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("~/Views/Company/Agents/Details.cshtml");
            }
        }
    }
}
