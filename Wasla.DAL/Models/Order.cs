using Wasla.Enums;

namespace Wasla.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string TrackingUuid { get; set; } = Guid.NewGuid().ToString();
        public string CustomerAddress { get; set; }
        public EgyptCity? CityFrom { get; set; }
        public EgyptCity CityTo { get; set; }
        public bool isClaimingRequired { get; set; }
        public bool isBreakable { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus status { get; set; } = OrderStatus.PendingConfirmation;
        public PaymentType PaymentType { get; set; } = PaymentType.COD;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public int MerchantId { get; set; }
        public virtual Merchant Merchant { get; set; }

        public int? CompanyId { get; set; }
        public virtual CourierCompany Company { get; set; }

        public int? DriverId { get; set; }
        public virtual Driver Driver { get; set; }

        public virtual List<OrderProduct> OrderProducts { get; set; }
        public virtual List<TrackingHistory> TrackingHistories { get; set; }
        public OrderCompany orderCompany { get; set; }
        public DriverOrder DriverOrder { get; set; }
    }
}