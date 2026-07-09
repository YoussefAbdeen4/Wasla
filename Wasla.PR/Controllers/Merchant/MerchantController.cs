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
    public class MerchantController : Controller
    {
        private readonly MerchantService _merchantService;

        public MerchantController(MerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var dto = await _merchantService.GetDashboardAsync(userId, 5);

            var model = new MerchantDashboardViewModel
            {
                TotalOrdersCount = dto.TotalOrdersCount,
                DeliveredOrdersCount = dto.DeliveredOrdersCount,
                RejectedOrdersCount = dto.RejectedOrdersCount,
                UnderReviewOrdersCount = dto.UnderReviewOrdersCount,
                TotalSales = dto.TotalSales,
                RecentOrders = dto.RecentOrders
            };

            return View(model);
        }

        public async Task<IActionResult> Orders(int page = 1)
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var merchant = await _merchantService.GetMerchantByUserIdAsync(userId);
            if (merchant == null)
                return Forbid();

            var orders = await _merchantService.GetRecentOrdersAsync(merchant.Id, take: 100);

            var list = orders.Select(o => new PR.ViewModels.Merchant.OrderPreviewViewModel
            {
                Id = o.Id,
                CustomerName = o.CustomerName,
                CustomerPhone = o.CustomerPhone,
                TotalPrice = o.TotalPrice,
                Status = o.status.ToString(),
                CreatedAt = o.CreatedAt
            }).ToList();

            return View(list);
        }

        public IActionResult AddOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(PR.ViewModels.Merchant.AddOrderViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var merchant = await _merchantService.GetMerchantByUserIdAsync(userId);
            if (merchant == null)
                return Forbid();

            var order = new Order
            {
                CustomerName = model.CustomerName,
                CustomerPhone = model.CustomerPhone,
                CustomerAddress = model.CustomerAddress,
                CityFrom = model.CityFrom,
                CityTo = model.CityTo,
                isClaimingRequired = model.IsClaimingRequired,
                isBreakable = model.IsBreakable,
                TotalPrice = model.TotalPrice,
                PaymentType = model.PaymentType,
                status = Enums.OrderStatus.Created
            };

            await _merchantService.CreateOrderAsync(merchant.Id, order);

            return RedirectToAction("Orders");
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var order = await _merchantService.GetOrderDetailsAsync(id, userId);
            if (order == null)
                return NotFound();

            // Simple mapping to existing Company OrderDetailsViewModel
            var vm = new PR.ViewModels.Company.OrderDetailsViewModel
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
                OrderProducts = order.OrderProducts?.Select(op => new PR.ViewModels.Company.OrderProductViewModel
                {
                    Id = op.Id,
                    Name = op.Name,
                    Price = op.Price,
                    Qty = op.Qty
                }).ToList() ?? new List<PR.ViewModels.Company.OrderProductViewModel>(),
                FinancialDetails = new PR.ViewModels.Company.FinancialDetailsViewModel
                {
                    BaseFee = 0,
                    TotalPrice = order.TotalPrice,
                    ExtraCharges = 0,
                    DriverCommission = 0,
                    CompanyProfit = 0,
                    PaymentType = order.PaymentType,
                    PaymentStatus = ""
                },
                CurrentDriverId = order.DriverId,
                CurrentDriverName = order.Driver?.Name,
                TrackingHistory = order.TrackingHistories?.Select(th => new PR.ViewModels.Company.TrackingHistoryViewModel
                {
                    Id = th.Id,
                    Status = th.Status,
                    Timestamp = th.Timestamp,
                    Location = th.Location
                }).ToList() ?? new List<PR.ViewModels.Company.TrackingHistoryViewModel>()
            };

            return View(vm);
        }
    }
}
