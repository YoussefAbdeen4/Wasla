
using Wasla.Enums;

namespace Wasla.Models
{
   
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string TrackingUuid { get; set; }
        public string CustomerAddress { get; set; }
        public EgyptCity CityFrom { get; set; } // enum
        public EgyptCity CityTo { get; set; } // enum
        public bool isClaimingRequired { get; set; }    
        public bool isBreakable { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentType PaymentType { get; set; } // enum
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeliveredAt { get; set; }

        public int MerchantId { get; set; }
        public virtual Merchant Merchant { get; set; }

        public int CompanyId { get; set; }
        public virtual CourierCompany Company { get; set; }

        public int DriverId { get; set; }
        public virtual Driver Driver { get; set; }

        public virtual List <OrderProduct> OrderProducts { get; set; }
        public virtual List <TrackingHistory> TrackingHistories { get; set; }
        public OrderCompany orderCompany { get; set; }
        public DriverOrder DriverOrder { get; set; }
    }
}
