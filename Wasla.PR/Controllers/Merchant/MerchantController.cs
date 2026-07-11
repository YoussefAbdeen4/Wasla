using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Wasla.Models;
using Wasla.PR.ViewModels;
using Wasla.BLL.Services;

namespace Wasla.Controllers
{
    [Authorize]
    public class MerchantController : Controller
    {
        private readonly MerchantService _merchantService;

        public MerchantController(MerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        // ==================== Dashboard ====================
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var dto = await _merchantService.GetDashboardAsync(userId, 5);

            var model = new MerchantDashboardViewModel
            {
                TotalOrdersCount     = dto.TotalOrdersCount,
                DeliveredOrdersCount = dto.DeliveredOrdersCount,
                RejectedOrdersCount  = dto.RejectedOrdersCount,
                UnderReviewOrdersCount = dto.UnderReviewOrdersCount,
                TotalSales           = dto.TotalSales,
                RecentOrders         = dto.RecentOrders
            };

            return View("~/Views/Merchant/Dashboard/Index.cshtml", model);
        }

        // ==================== Orders ====================
        public async Task<IActionResult> Orders(int page = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var merchant = await _merchantService.GetMerchantByUserIdAsync(userId);
            if (merchant == null)
                return Forbid();

            var orders = await _merchantService.GetRecentOrdersAsync(merchant.Id, take: 100);

            var list = orders.Select(o => new PR.ViewModels.Merchant.OrderPreviewViewModel
            {
                Id           = o.Id,
                CustomerName = o.CustomerName,
                CustomerPhone= o.CustomerPhone,
                TotalPrice   = o.TotalPrice,
                Status       = o.status.ToString(),
                CreatedAt    = o.CreatedAt
            }).ToList();

            return View("~/Views/Merchant/Orders/Index.cshtml", list);
        }

        // ==================== Add Order ====================
        public IActionResult AddOrder()
        {
            return View("~/Views/Merchant/Orders/Add.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrder(PR.ViewModels.Merchant.AddOrderViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Merchant/Orders/Add.cshtml", model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var merchant = await _merchantService.GetMerchantByUserIdAsync(userId);
            if (merchant == null)
                return Forbid();

            var order = new Order
            {
                CustomerName     = model.CustomerName,
                CustomerPhone    = model.CustomerPhone,
                CustomerAddress  = model.CustomerAddress,
                CityFrom         = model.CityFrom,
                CityTo           = model.CityTo,
                isClaimingRequired = model.IsClaimingRequired,
                isBreakable      = model.IsBreakable,
                TotalPrice       = model.TotalPrice,
                PaymentType      = model.PaymentType,
                status           = Enums.OrderStatus.Created
            };

            await _merchantService.CreateOrderAsync(merchant.Id, order);

            TempData["Success"] = "تم إنشاء الطلب بنجاح";
            return RedirectToAction("Orders");
        }

        // ==================== Order Details ====================
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _merchantService.GetOrderDetailsAsync(id, userId);
            if (order == null)
                return NotFound();

            var vm = new PR.ViewModels.Merchant.MerchantOrderDetailsViewModel
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
                TotalPrice = order.TotalPrice,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                DeliveredAt = order.DeliveredAt,
                CurrentDriverName = order.Driver?.Name,
                TrackingHistory = order.TrackingHistories?.Select(th => new PR.ViewModels.Merchant.MerchantTrackingItemViewModel
                {
                    Id = th.Id,
                    Status = th.Status.ToString(),
                    Timestamp = th.Timestamp,
                    Location = th.Location
                }).ToList() ?? new List<PR.ViewModels.Merchant.MerchantTrackingItemViewModel>()
            };

            if (order.CompanyId == null)
            {
                ViewBag.AvailableCouriers = await _merchantService.GetAvailableCouriersForOrderAsync(id);
            }
            else
            {
                ViewBag.AssignedCompanyName = order.Company?.CompanyName ?? order.Company?.Name;
            }

            return View("~/Views/Merchant/Orders/Details.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignCourier(int orderId, int companyId)
        {
            var success = await _merchantService.AssignCompanyToOrderAsync(orderId, companyId);
            if (success)
            {
                TempData["Success"] = "تم إسناد طلب الشحن للشركة بنجاح.";
            }
            else
            {
                TempData["Error"] = "حدث خطأ أثناء إسناد الشركة.";
            }
            return RedirectToAction("Details", new { id = orderId });
        }

        // ==================== Wallet ====================
        public async Task<IActionResult> Wallet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var merchant = await _merchantService.GetMerchantByUserIdAsync(userId);

            var vm = new PR.ViewModels.Merchant.WalletViewModel
            {
                AvailableBalance = merchant?.WalletBalance ?? 0m,
                PendingClearance = 0m,
                TotalWithdrawn   = 0m,
                TotalRevenue     = merchant != null
                    ? (await _merchantService.GetTotalRevenueAsync(merchant.Id))
                    : 0m
            };

            return View("~/Views/Merchant/Wallet/Index.cshtml", vm);
        }

        // ==================== Profile ====================
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var merchant = await _merchantService.GetMerchantByUserIdAsync(userId);
            if (merchant == null)
                return Forbid();

            var vm = new PR.ViewModels.Merchant.MerchantProfileViewModel
            {
                Id        = merchant.Id,
                Name      = merchant.Name,
                StoreName = merchant.StoreName,
                TaxNumber = merchant.TaxNumber,
                Category  = merchant.Category
            };

            return View("~/Views/Merchant/Profile/Index.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(PR.ViewModels.Merchant.MerchantProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Merchant/Profile/Index.cshtml", model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var updated = new Merchant
            {
                Name      = model.Name,
                StoreName = model.StoreName,
                TaxNumber = model.TaxNumber,
                Category  = model.Category
            };

            await _merchantService.UpdateMerchantProfileAsync(userId, updated);

            TempData["Success"] = "تم حفظ الملف الشخصي بنجاح";
            return RedirectToAction("Profile");
        }

        // ==================== Settings ====================
        public IActionResult Settings()
        {
            return View("~/Views/Merchant/Settings/Index.cshtml");
        }

        // ==================== Notifications ====================
        public IActionResult Notifications()
        {
            return View("~/Views/Merchant/Notifications/Index.cshtml");
        }
    }
}
