using Microsoft.EntityFrameworkCore;
using Wasla.Enums;
using Wasla.Models;

namespace Wasla.BLL.Services
{
    public class CompanyFinancialsService
    {
        private readonly AppDbContext _context;

        public CompanyFinancialsService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get financial summary and transaction history for company
        /// </summary>
        public async Task<(FinancialSummary summary, List<TransactionHistory> transactions)> GetFinancialsAsync(int companyId, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                // Get all company orders with related data
                var orders = await _context.Orders
                    .Where(o => o.CompanyId == companyId)
                    .Include(o => o.Merchant)
                    .OrderByDescending(o => o.CreatedAt)
                    .ToListAsync();

                // Calculate financial summary
                var summary = new FinancialSummary
                {
                    TotalBalance = orders.Sum(o => o.TotalPrice),
                    ExpectedCollection = orders
                        .Where(o => o.PaymentType == PaymentType.COD && 
                                   (o.status == OrderStatus.OutForDelivery || 
                                    o.status == OrderStatus.PendingConfirmation ||
                                    o.status == OrderStatus.Created ||
                                    o.status == OrderStatus.Packed))
                        .Sum(o => o.TotalPrice),
                    DeliveredToCompany = orders
                        .Where(o => o.status == OrderStatus.Delivered)
                        .Sum(o => o.TotalPrice),
                    PendingAmount = orders
                        .Where(o => (o.status == OrderStatus.OutForDelivery || 
                                    o.status == OrderStatus.Packed ||
                                    o.status == OrderStatus.PendingConfirmation) &&
                                   o.PaymentType != PaymentType.COD)
                        .Sum(o => o.TotalPrice)
                };

                // Get transaction history (paginated)
                var transactions = orders
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(o => new TransactionHistory
                    {
                        Id = o.Id,
                        OrderNumber = o.TrackingUuid,
                        CustomerName = o.CustomerName,
                        Amount = o.TotalPrice,
                        PaymentType = o.PaymentType,
                        OrderStatus = o.status,
                        CreatedAt = o.CreatedAt
                    })
                    .ToList();

                return (summary, transactions);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في جلب البيانات المالية: {ex.Message}");
            }
        }

        /// <summary>
        /// Get financial metrics for a specific date range
        /// </summary>
        public async Task<FinancialSummary> GetFinancialsSummaryByDateAsync(int companyId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.CompanyId == companyId &&
                               o.CreatedAt >= startDate &&
                               o.CreatedAt <= endDate)
                    .ToListAsync();

                return new FinancialSummary
                {
                    TotalBalance = orders.Sum(o => o.TotalPrice),
                    ExpectedCollection = orders
                        .Where(o => o.PaymentType == PaymentType.COD && 
                                   (o.status == OrderStatus.OutForDelivery || 
                                    o.status == OrderStatus.PendingConfirmation ||
                                    o.status == OrderStatus.Created ||
                                    o.status == OrderStatus.Packed))
                        .Sum(o => o.TotalPrice),
                    DeliveredToCompany = orders
                        .Where(o => o.status == OrderStatus.Delivered)
                        .Sum(o => o.TotalPrice),
                    PendingAmount = orders
                        .Where(o => (o.status == OrderStatus.OutForDelivery || 
                                    o.status == OrderStatus.Packed ||
                                    o.status == OrderStatus.PendingConfirmation) &&
                                   o.PaymentType != PaymentType.COD)
                        .Sum(o => o.TotalPrice)
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في حساب الميزانية: {ex.Message}");
            }
        }
    }

    public class FinancialSummary
    {
        public decimal TotalBalance { get; set; } // الرصيد الإجمالي
        public decimal ExpectedCollection { get; set; } // المتوقع تحصيله
        public decimal DeliveredToCompany { get; set; } // المسلم للشركة
        public decimal PendingAmount { get; set; } // المعلقة
    }

    public class TransactionHistory
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
