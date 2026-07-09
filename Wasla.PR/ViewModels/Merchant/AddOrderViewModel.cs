using System;
using System.ComponentModel.DataAnnotations;
using Wasla.Enums;

namespace Wasla.PR.ViewModels.Merchant
{
    public class AddOrderViewModel
    {
        [Required]
        public string CustomerName { get; set; }

        [Required]
        [Phone]
        public string CustomerPhone { get; set; }

        [Required]
        public string CustomerAddress { get; set; }

        public EgyptCity? CityFrom { get; set; }
        public EgyptCity CityTo { get; set; }

        public bool IsClaimingRequired { get; set; }
        public bool IsBreakable { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }

        public PaymentType PaymentType { get; set; } = PaymentType.COD;
    }
}
