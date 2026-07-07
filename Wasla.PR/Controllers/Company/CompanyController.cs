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
using Wasla.PR.ViewModels.Company;

namespace Wasla.PR.Controllers.Company
{
    [Authorize]
    public class CompanyController : Controller
    {
        #region dependencies
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OrderServices _orderService;
        private readonly CompanyService _companyService;
        private readonly CompanyFinancialsService _financialsService;
        private readonly CompanyDashboardService _dashboardService; 
        private readonly AppDbContext _context; 
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompanyController(
            UserManager<ApplicationUser> userManager,
            OrderServices orderService,
            CompanyService companyService,
            CompanyFinancialsService financialsService,
            CompanyDashboardService dashboardService, 
            AppDbContext context, 
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _orderService = orderService;
            _companyService = companyService;
            _financialsService = financialsService;
            _dashboardService = dashboardService; 
            _context = context; 
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Index
        // GET: CompanyController
        public async Task<IActionResult> Index()
        {
            // 1. جلب المستخدم الحالي
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Auth");

            // 2. جلب الشركة المرتبطة بالمستخدم
            var company = await _context.CourierCompanies
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            // إذا المستخدم ملوش شركة، وجهه لصفحة تعريف الشركة أو خطأ
            if (company == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // 3. جلب البيانات من السيرفيس
            var (stats, recentOrders, drivers) = await _dashboardService.GetDashboardDataAsync(company.Id);

            // 4. تعبئة الـ ViewModel (Mapping)
            var viewModel = new DashboardIndexViewModel
            {
                Statistics = new DashboardStatisticsViewModel
                {
                    TotalOrders = stats.TotalOrders,
                    PendingOrders = stats.PendingOrders,
                    OutForDeliveryOrders = stats.OutForDeliveryOrders,
                    DeliveredOrders = stats.DeliveredOrders,
                    FailedOrders = stats.FailedOrders,
                    TotalRevenue = stats.TotalRevenue,
                    TotalDrivers = stats.TotalDrivers,
                    TotalAgents = stats.TotalAgents
                },
                RecentOrders = recentOrders.Select(o => new RecentOrderViewModel
                {
                    Id = o.Id,
                    TrackingUuid = o.TrackingUuid,
                    CustomerName = o.CustomerName,
                    DriverName = o.DriverName,
                    TotalPrice = o.TotalPrice,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt
                }).ToList(),
                AvailableDrivers = drivers.Select(d => new AvailableDriverViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Phone = d.Phone,
                    ActiveOrders = d.ActiveOrders
                }).ToList()
            };

            return View("~/Views/Company/Dashboard/Index.cshtml",viewModel);
        }
        #endregion

        #region Orders
        public async Task<IActionResult> Orders()
        {
            var companyUserId = _userManager.GetUserId(User);

            var orders = await _orderService.GetCompanyOrdersAsync(companyUserId);

            var vm = new AllOrdersViewModel
            {
                TotalOrders = orders.Count,

                PendingOrders = orders.Count(x =>
                    x.status == OrderStatus.Created),

                OutForDeliveryOrders = orders.Count(x =>
                    x.status == OrderStatus.OutForDelivery),

                DeliveredOrders = orders.Count(x =>
                    x.status == OrderStatus.Delivered),

                ReturnedOrders = orders.Count(x =>
                    x.status == OrderStatus.Returned),

                WaitingApprovalOrders = orders.Count(x =>
                    x.status == OrderStatus.PendingConfirmation),

                LatestOrders = orders.Select(x => new OrderItemViewModel
                {
                    Id = x.Id,
                    OrderNumber = x.TrackingUuid,
                    CustomerName = x.CustomerName,
                    DriverName = x.Driver?.Name ?? "غير مسند",
                    Amount = x.TotalPrice,
                    Status = x.status
                }).ToList()
            };

            return View("~/Views/Company/Orders/Index.cshtml", vm);
        }

        /// <summary>
        /// GET: Display full order details
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var company = await _companyService.GetCompanyByUserIdAsync(userId);

                var order = await _orderService.GetOrderDetailsAsync(id, company.Id);

                // Get available drivers
                var drivers = await _orderService.GetCompanyDriversAsync(company.Id);

                // Map to ViewModel
                var viewModel = new OrderDetailsViewModel
                {
                    Id = order.Id,
                    TrackingUuid = order.TrackingUuid,
                    CustomerName = order.CustomerName,
                    CustomerPhone = order.CustomerPhone,
                    CustomerAddress = order.CustomerAddress,
                    CityFrom = order.CityFrom,
                    CityTo = order.CityTo,
                    IsClaimingRequired = order.isClaimingRequired,
                    IsBreakable = order.isBreakable,
                    Status = order.status,
                    PaymentType = order.PaymentType,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    DeliveredAt = order.DeliveredAt,
                    CurrentDriverId = order.DriverId,
                    CurrentDriverName = order.Driver?.Name,
                    OrderProducts = order.OrderProducts?.Select(p => new OrderProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Qty = p.Qty,
                        Price = p.Price
                    }).ToList() ?? new List<OrderProductViewModel>(),
                    AvailableDrivers = drivers.Select(d => new DriverOptionViewModel
                    {
                        Id = d.Id,
                        Name = d.Name,
                        IsAvailable = true
                    }).ToList(),
                    TrackingHistory = order.TrackingHistories?.Select(t => new TrackingHistoryViewModel
                    {
                        Id = t.Id,
                        Status = t.Status,
                        Timestamp = t.Timestamp,
                        Location = t.Location
                    }).OrderByDescending(t => t.Timestamp).ToList() ?? new List<TrackingHistoryViewModel>(),
                    FinancialDetails = new FinancialDetailsViewModel
                    {
                        TotalPrice = order.TotalPrice,
                        PaymentType = order.PaymentType,
                        PaymentStatus = order.PaymentType == PaymentType.COD ? "الدفع عند الاستلام" : "مدفوع"
                    }
                };

                return View("~/Views/Company/Orders/OrderDetails.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Orders");
            }
        }

        /// <summary>
        /// POST: Change order status
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Company/ChangeOrderStatus")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, OrderStatus newStatus)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var company = await _companyService.GetCompanyByUserIdAsync(userId);

                await _orderService.ChangeOrderStatusAsync(orderId, company.Id, newStatus);

                TempData["Success"] = "تم تغيير حالة الطلب بنجاح";
                return RedirectToAction("OrderDetails", new { id = orderId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("OrderDetails", new { id = orderId });
            }
        }

        /// <summary>
        /// POST: Assign driver to order
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignDriver(int orderId, int driverId)
        {
            try
            {
                if (driverId <= 0)
                    throw new Exception("يجب اختيار سائق");

                var userId = _userManager.GetUserId(User);
                var company = await _companyService.GetCompanyByUserIdAsync(userId);

                await _orderService.AssignDriverToOrderAsync(orderId, company.Id, driverId);

                TempData["Success"] = "تم إسناد الطلب للسائق بنجاح";
                return RedirectToAction("OrderDetails", new { id = orderId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("OrderDetails", new { id = orderId });
            }
        }

        #endregion


        #region Profile Management

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var company = await _companyService.GetCompanyByUserIdAsync(user.Id);
            var rateCards = await _companyService.GetRateCardsByCompanyIdAsync(company.Id); 

            var viewModel = new CompanyProfileViewModel
            {
                Id = company.Id,
                CompanyName = company.CompanyName,
                Name = company.Name,
                TaxNumber = company.TaxNumber,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DeliveryZones = rateCards.Select(r => r.DestinationCity.ToString()).Distinct().ToList()
            };
            return View("~/Views/Company/Profile/Index.cshtml", viewModel);
        }

        /// <summary>
        /// GET: Display company profile for editing
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var company = await _companyService.GetCompanyByUserIdAsync(userId);

                var viewModel = new CompanyProfileViewModel
                {
                    Id = company.Id,
                    CompanyName = company.CompanyName,
                    Name = company.Name,
                    TaxNumber = company.TaxNumber,
                    Email = company.User?.Email,
                    PhoneNumber = company.User?.PhoneNumber,
                    ExistingImagePath = null
                };

                return View("~/Views/Company/Profile/EditProfile.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء تحميل بيانات الملف الشخصي";
                return RedirectToAction("Profile");
            }
        }

        /// <summary>
        /// POST: Update company profile with image upload
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(CompanyProfileViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Views/Company/Profile/EditProfile.cshtml", model);
                }

                var userId = _userManager.GetUserId(User);
                var company = await _companyService.GetCompanyByUserIdAsync(userId);

                // Handle image upload
                string imagePath = model.ExistingImagePath;
                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    imagePath = await SaveProfileImageAsync(model.ProfileImage);
                }

                // Map ViewModel to Model
                company.CompanyName = model.CompanyName;
                company.Name = model.Name;
                company.TaxNumber = model.TaxNumber;

                // Update ApplicationUser
                var user = await _userManager.GetUserAsync(User);
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                // Update database
                await _companyService.UpdateCompanyProfileAsync(company);
                await _userManager.UpdateAsync(user);

                TempData["Success"] = "تم تحديث بيانات الملف الشخصي بنجاح";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"حدث خطأ أثناء تحديث البيانات: {ex.Message}";
                return View("~/Views/Company/Profile/EditProfile.cshtml", model);
            }
        }

        #endregion

 
        #region Helper Methods

        /// <summary>
        /// Save uploaded profile image to wwwroot/images/companies
        /// </summary>
        private async Task<string> SaveProfileImageAsync(IFormFile image)
        {
            try
            {
                if (image == null || image.Length == 0)
                    return null;

                // Validate file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(image.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                    throw new Exception("نوع الملف غير مدعوم. الملفات المسموحة: jpg, jpeg, png, gif");

                if (image.Length > 5 * 1024 * 1024) // 5MB
                    throw new Exception("حجم الصورة يجب أن يكون أقل من 5 ميجابايت");

                // Create directory if not exists
                var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images", "companies");
                Directory.CreateDirectory(uploadsDirectory);

                // Generate unique filename
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
                var filePath = Path.Combine(uploadsDirectory, uniqueFileName);

                // Save file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                // Return relative path for database storage
                return $"/images/companies/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في رفع الصورة: {ex.Message}");
            }
        }

        #endregion

        #region Financials
        public async Task<IActionResult> Financials()
        {
            try
            {
                // افترض إنك بتجيب الـ companyId من الـ Session أو Claims
                int companyId = 1;

                // 1. استدعاء السيرفيس
                var (summary, transactions) = await _financialsService.GetFinancialsAsync(companyId);

                // 2. تعبئة الـ ViewModel
                var viewModel = new FinancialsViewModel
                {
                    Summary = new FinancialSummaryViewModel
                    {
                        TotalBalance = summary.TotalBalance,
                        ExpectedCollection = summary.ExpectedCollection,
                        DeliveredToCompany = summary.DeliveredToCompany,
                        PendingAmount = summary.PendingAmount
                    },
                    Transactions = transactions.Select(t => new TransactionHistoryViewModel
                    {
                        Id = t.Id,
                        OrderNumber = t.OrderNumber,
                        CustomerName = t.CustomerName,
                        Amount = t.Amount,
                        PaymentType = t.PaymentType,
                        OrderStatus = t.OrderStatus,
                        CreatedAt = t.CreatedAt
                    }).ToList()
                };

                return View("~/Views/Company/Financials/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء تحميل البيانات المالية.";
                return RedirectToAction("Index", "CompanyDashboard");
            }
        }
        #endregion
    }
}
