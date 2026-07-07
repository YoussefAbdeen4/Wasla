using Wasla.Enums;

namespace Wasla.PR.ViewModels.Company
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }
        public string TrackingUuid { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public EgyptCity? CityFrom { get; set; }
        public EgyptCity CityTo { get; set; }
        public bool IsClaimingRequired { get; set; }
        public bool IsBreakable { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        // Order Items
        public List<OrderProductViewModel> OrderProducts { get; set; } = new();

        // Financial Details
        public FinancialDetailsViewModel FinancialDetails { get; set; }

        // Driver Assignment
        public int? CurrentDriverId { get; set; }
        public string CurrentDriverName { get; set; }
        public List<DriverOptionViewModel> AvailableDrivers { get; set; } = new();

        // Tracking History
        public List<TrackingHistoryViewModel> TrackingHistory { get; set; } = new();
    }

    public class OrderProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }

    public class FinancialDetailsViewModel
    {
        public decimal BaseFee { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ExtraCharges { get; set; }
        public decimal DriverCommission { get; set; }
        public decimal CompanyProfit { get; set; }
        public PaymentType PaymentType { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class DriverOptionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class TrackingHistoryViewModel
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
        public string Location { get; set; }
    }
}
