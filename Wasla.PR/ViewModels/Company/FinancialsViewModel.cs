using Wasla.Enums;

namespace Wasla.PR.ViewModels.Company
{
    public class FinancialsViewModel
    {
        public FinancialSummaryViewModel Summary { get; set; }
        public List<TransactionHistoryViewModel> Transactions { get; set; } = new();
    }

    public class FinancialSummaryViewModel
    {
        public decimal TotalBalance { get; set; } // الرصيد الإجمالي
        public decimal ExpectedCollection { get; set; } // المتوقع تحصيله (COD Orders)
        public decimal DeliveredToCompany { get; set; } // المسلم للشركة (Delivered Orders)
        public decimal PendingAmount { get; set; } // المعلقة (Pending Orders)
    }

    public class TransactionHistoryViewModel
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
