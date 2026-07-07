using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wasla.Models;
using Wasla.Enums;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Wasla.PR.ViewModels.Agent;

namespace Wasla.PR.Controllers.Agent
{
    public class AgentController : Controller
    {
        private readonly AppDbContext _db;

        public AgentController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Agent or /Agent/Index
        public async Task<IActionResult> Index(int? driverId)
        {
            var id = await ResolveDriverId(driverId);
            if (id == null)
            {
                return View(new AgentDashboardViewModel());
            }

            var ordersQuery = _db.Orders
                .Where(o => o.Driver != null && o.Driver.Id == id)
                .Include(o => o.TrackingHistories)
                .OrderByDescending(o => o.CreatedAt);

            var total = await ordersQuery.CountAsync();
            var delivered = await ordersQuery.Where(o => o.DeliveredAt != default).CountAsync();
            var returned = await ordersQuery.Where(o => o.TrackingHistories.Any(th => th.Status == OrderStatus.Returned)).CountAsync();

            // financials calculation using COD
            var driver = await _db.Drivers.FindAsync(id);
            var codOrders = ordersQuery.Where(o => o.PaymentType == PaymentType.COD);

            var totalCashCollected = await codOrders.Where(o => o.DeliveredAt != default).SumAsync(o => (decimal?)o.TotalPrice) ?? 0m;
            var cashDeliveredToCompany = driver?.TotalCashSubmitted ?? 0m;
            var cashInHand = totalCashCollected - cashDeliveredToCompany;
            var balance = cashInHand;

            var recent = await ordersQuery.Take(5).Select(o => new OrderSummaryViewModel
            {
                Id = o.Id,
                OrderNumber = o.TrackingUuid,
                CustomerName = o.CustomerName,
                Amount = o.TotalPrice,
                CreatedAt = o.CreatedAt,
                Status = o.TrackingHistories.OrderByDescending(th => th.Timestamp).Select(th => th.Status.ToString()).FirstOrDefault() ?? (o.DeliveredAt != default ? "Delivered" : "Created")
            }).ToListAsync();

            var vm = new AgentDashboardViewModel
            {
                TotalOrders = total,
                Delivered = delivered,
                Returned = returned,
                CashInHand = cashInHand,
                CashDelivered = cashDeliveredToCompany,
                Balance = balance,
                RecentOrders = recent
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateOrderStatus(int orderId, string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return Json(new { success = false, message = "Status is required" });

            var order = await _db.Orders.Include(o => o.TrackingHistories).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
                return Json(new { success = false, message = "Order not found" });

            if (!Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
                return Json(new { success = false, message = "Invalid status" });

            var history = new TrackingHistory
            {
                OrderId = orderId,
                Status = parsedStatus,
                Location = string.Empty,
                Timestamp = DateTime.UtcNow
            };

            _db.TrackingHistories.Add(history);

            if (parsedStatus == OrderStatus.Delivered)
            {
                order.DeliveredAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Order status updated" });
        }

        [HttpPost]
        public async Task<JsonResult> SubmitCash(decimal amount)
        {
            var driverId = await ResolveDriverId(null);
            if (driverId == null) return Json(new { success = false, message = "Driver not found" });

            if (amount <= 0) return Json(new { success = false, message = "Invalid amount" });

            var driver = await _db.Drivers.FindAsync(driverId);
            if (driver == null) return Json(new { success = false, message = "Driver not found" });

            // Add the submitted money to the database and save
            driver.TotalCashSubmitted += amount;
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Cash submitted successfully", amount = amount });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAvatar(IFormFile avatar)
        {
            // placeholder: accept file but do not persist yet
            await Task.CompletedTask;
            return Json(new { success = true, message = "Avatar updated (placeholder)" });
        }

        // GET: /Agent/Orders
        public async Task<IActionResult> Orders(int? driverId)
        {
            var id = await ResolveDriverId(driverId);
            if (id == null) return View(new List<AgentOrderViewModel>());

            var orders = await _db.Orders
                .Where(o => o.Driver != null && o.Driver.Id == id)
                .Include(o => o.TrackingHistories)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new AgentOrderViewModel
                {
                    Id = o.Id,
                    OrderNumber = o.TrackingUuid,
                    CustomerName = o.CustomerName,
                    Amount = o.TotalPrice,
                    CreatedAt = o.CreatedAt,
                    Status = o.TrackingHistories.OrderByDescending(th => th.Timestamp).Select(th => th.Status.ToString()).FirstOrDefault() ?? (o.DeliveredAt != default ? "Delivered" : "Created"),
                    CanPickUp = true,
                    CanDeliver = o.DeliveredAt == default,
                    CanReturn = o.TrackingHistories.Any()
                })
                .ToListAsync();

            return View(orders);
        }

        // GET: /Agent/Financials
        public async Task<IActionResult> Financials(int? driverId)
        {
            var id = await ResolveDriverId(driverId);
            if (id == null) return View(new AgentFinancialsViewModel());

            var driver = await _db.Drivers.FindAsync(id);
            var ordersQuery = _db.Orders.Where(o => o.Driver != null && o.Driver.Id == id);

            // Calculate total cash collected from COD orders that are Delivered
            var codOrders = ordersQuery.Where(o => o.PaymentType == PaymentType.COD);
            var totalCashCollected = await codOrders.Where(o => o.DeliveredAt != default).SumAsync(o => (decimal?)o.TotalPrice) ?? 0m;

            // Calculate the agent's real balances
            var cashDeliveredToCompany = driver?.TotalCashSubmitted ?? 0m;
            var cashInHand = totalCashCollected - cashDeliveredToCompany;

            var vm = new AgentFinancialsViewModel
            {
                CashInHand = cashInHand,
                CashDelivered = cashDeliveredToCompany,
                Balance = cashInHand // The balance they still owe the company
            };

            return View(vm);
        }

        // GET: /Agent/Profile
        public async Task<IActionResult> Profile(int? driverId)
        {
            var id = await ResolveDriverId(driverId);
            if (id == null) return View(new AgentProfileViewModel());

            var driver = await _db.Drivers.Include(d => d.DriverVehicles).ThenInclude(dv => dv.Vehicle).FirstOrDefaultAsync(d => d.Id == id);
            if (driver == null) return View(new AgentProfileViewModel());

            var vehicleType = driver.DriverVehicles?.FirstOrDefault()?.Vehicle?.Type.ToString() ?? string.Empty;

            var vm = new AgentProfileViewModel
            {
                Id = driver.Id,
                FullName = driver.Name,
                Phone = driver.Phones?.FirstOrDefault()?.PhoneNumber ?? string.Empty,
                VehicleType = vehicleType,
                AvatarUrl = string.Empty,
               // Email = driver.Email
            };

            return View(vm);
        }

        private async Task<int?> ResolveDriverId(int? providedId)
        {
            if (providedId.HasValue) return providedId.Value;

            // try to read claim (if authentication exists)
            if (User?.Identity?.IsAuthenticated == true)
            {
                var claim = User.Claims.FirstOrDefault(c => c.Type == "DriverId" || c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
                if (claim != null && int.TryParse(claim.Value, out var parsed)) return parsed;
            }

            // fallback: use first driver in database for development
            var first = await _db.Drivers.OrderBy(d => d.Id).FirstOrDefaultAsync();
            return first?.Id;
        }
    }
}