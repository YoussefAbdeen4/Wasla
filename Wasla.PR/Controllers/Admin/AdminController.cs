using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wasla.BLL.Services;
using Wasla.BLL.Services.Admin;
using Wasla.PR.ViewModels.Admin;

namespace Wasla.PR.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminDashboardService _adminDashboardService;
        private readonly AdminOrdersService _adminOrdersService;
        private readonly AdminMerchantsService _adminMerchantsService;
        private readonly AdminDriversService _adminDriversService;

        public AdminController(
            AdminDashboardService adminDashboardService,
           AdminOrdersService adminOrdersService,
            AdminMerchantsService adminMerchantsService,
            AdminDriversService adminDriversService)
        {
            _adminDashboardService = adminDashboardService;
            _adminOrdersService = adminOrdersService;
            _adminMerchantsService = adminMerchantsService;
            _adminDriversService=adminDriversService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var (totalCompanies, totalMerchants, totalOrders, TotalDrivers) =
                await _adminDashboardService.GetGlobalStatisticsAsync();

            var recentOrders = await _adminDashboardService.GetRecentOrdersAsync(10);
            var recentCompanies = await _adminDashboardService.GetRecentCompaniesAsync(10);

            var vm = new AdminDashboardViewModel
            {
                TotalCompanies = totalCompanies,
                TotalMerchants = totalMerchants,
                TotalOrders = totalOrders,
                TotalDrivers = TotalDrivers,

                RecentOrders = recentOrders.Select(o => new AdminRecentOrderViewModel
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName,
                    CreatedAt = o.CreatedAt,
                    Status = o.status.ToString()
                }).ToList(),

                RecentCompanies = recentCompanies.Select(c => new AdminCompanyListItemViewModel
                {
                    Id = c.Id,
                    Name = c.CompanyName,
                    Contact = c.Phones != null && c.Phones.Any()
                        ? c.Phones.First().PhoneNumber
                        : (c.User != null ? c.User.PhoneNumber : "-"),
                    OrdersCount = c.Orders != null ? c.Orders.Count : 0,
                    Status = _adminDashboardService.GetAccountStatus(c.User?.LockoutEnd)
                }).ToList()
            };

            return View("Dashboard/Index", vm);
        }

        public async Task<IActionResult> Companies()
        {
            var companies = await _adminDashboardService.GetAllCompaniesAsync();

            var vm = new AdminCompaniesListViewModel
            {
                Companies = companies.Select(c => new AdminCompanyListItemViewModel
                {
                    Id = c.Id,
                    Name = c.CompanyName,
                    Contact = c.Phones != null && c.Phones.Any()
                        ? c.Phones.First().PhoneNumber
                        : (c.User != null ? c.User.PhoneNumber : "-"),
                    OrdersCount = c.Orders != null ? c.Orders.Count : 0,
                    Status = _adminDashboardService.GetAccountStatus(c.User?.LockoutEnd)
                }).ToList()
            };

            return View("Companies/Index", vm);
        }
        public async Task<IActionResult> Merchants()
        {
            var merchants = await _adminMerchantsService.GetAllMerchantsAsync();

            var vm = new AdminMerchantsListViewModel
            {
                Merchants = merchants.Select(m => new AdminMerchantListItemViewModel
                {
                    Id = m.Id,
                    StoreName = m.StoreName,
                    Phone = m.Phones != null && m.Phones.Any()
                        ? m.Phones.First().PhoneNumber
                        : (m.User != null ? m.User.PhoneNumber : "-"),
                    OrdersCount = m.Orders != null ? m.Orders.Count : 0,
                    Status = _adminMerchantsService.GetAccountStatus(m.User?.LockoutEnd)
                }).ToList()
            };

            return View("Merchants/Index", vm);
        }
        public async Task<IActionResult> MerchantDetails(int id)
        {
            var merchant = await _adminMerchantsService.GetMerchantDetailsAsync(id);

            if (merchant == null)
                return NotFound();

            var vm = new AdminMerchantDetailsViewModel
            {
                Id = merchant.Id,
                Name = merchant.Name,
                StoreName = merchant.StoreName,
                TaxNumber = merchant.TaxNumber,
                Category = merchant.Category,
                WalletBalance = merchant.WalletBalance,
                Phones = merchant.Phones?.Select(p => p.PhoneNumber).ToList() ?? new(),
                Status = _adminMerchantsService.GetAccountStatus(merchant.User?.LockoutEnd),
                OrdersCount = merchant.Orders?.Count ?? 0,
                RatingsCount = merchant.Ratings?.Count ?? 0,
                ClientsCount = merchant.Clients?.Count ?? 0
            };

            return View("Merchants/Details", vm);
        }
        public async Task<IActionResult> Orders()
        {
            var orders = await _adminOrdersService.GetAllOrdersAsync();

            var vm = new AdminOrdersListViewModel
            {
                Orders = orders.Select(o => new AdminOrderListItemViewModel
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName,
                    CreatedAt = o.CreatedAt,
                    Status = o.status.ToString()
                }).ToList()
            };

            return View("Orders/Index", vm);
        }
        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _adminOrdersService.GetOrderDetailsAsync(id);

            if (order == null)
                return NotFound();

            var vm = new AdminOrderDetailsViewModel
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone,
                CustomerAddress = order.CustomerAddress,
                Status = order.status.ToString(),
                TotalPrice = order.TotalPrice,
                PaymentType = order.PaymentType.ToString(),
                CreatedAt = order.CreatedAt,
                DeliveredAt = order.DeliveredAt,
                MerchantStoreName = order.Merchant?.StoreName ?? "-",
                CompanyName = order.Company?.CompanyName ?? "-",
                DriverName = order.Driver?.Name ?? "غير مسند",
                ProductsCount = order.OrderProducts?.Count ?? 0
            };


            return View("~/Views/Admin/Orders/Deatils.cshtml", vm);
        }


        public async Task<IActionResult> Drivers()
        {
            var drivers = await _adminDriversService.GetAllDriversAsync();

            var vm = new AdminDriversListViewModel
            {
                Drivers = drivers.Select(d => new AdminDriverListItemViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Phone = d.Phones != null && d.Phones.Any()
                        ? d.Phones.First().PhoneNumber
                        : (d.User != null ? d.User.PhoneNumber : "-"),
                    Status = _adminDriversService.GetAccountStatus(d.User?.LockoutEnd)
                }).ToList()
            };

            return View("Drivers/Index", vm);
        }
        public async Task<IActionResult> DriverDetails(int id)
        {
            var driver = await _adminDriversService.GetDriverDetailsAsync(id);

            if (driver == null)
                return NotFound();

            var vm = new AdminDriverDetailsViewModel
            {
                Id = driver.Id,
                Name = driver.Name,
                CompanyName = driver.Company?.CompanyName ?? "-",
                TotalCashSubmitted = driver.TotalCashSubmitted,
                Phones = driver.Phones?.Select(p => p.PhoneNumber).ToList() ?? new(),
                Status = _adminDriversService.GetAccountStatus(driver.User?.LockoutEnd),
                OrdersCount = driver.Orders?.Count ?? 0,
                VehiclesCount = driver.DriverVehicles?.Count ?? 0
            };

            return View("Drivers/Details", vm);
        }

    }
}
