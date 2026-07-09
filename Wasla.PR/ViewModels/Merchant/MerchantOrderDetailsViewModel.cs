using System;
using System.Collections.Generic;
using Wasla.Enums;

namespace Wasla.PR.ViewModels.Merchant
{
    public class MerchantOrderDetailsViewModel
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
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public string CurrentDriverName { get; set; }

        public List<MerchantTrackingItemViewModel> TrackingHistory { get; set; } = new();
    }

    public class MerchantTrackingItemViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
        public string Location { get; set; }
    }
}
