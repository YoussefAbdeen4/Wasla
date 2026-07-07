using System.ComponentModel.DataAnnotations;
using Wasla.Enums;

namespace Wasla.PR.ViewModels.RateCards
{
    public class CreateRateCardViewModel
    {
        [Display(Name = "Origin City")]
        [Required(ErrorMessage = "Origin City is required")]
        public EgyptCity OriginCity { get; set; }

        [Display(Name = "Destination City")]
        [Required(ErrorMessage = "Destination City is required")]
        public EgyptCity DestinationCity { get; set; }

        [Display(Name = "Minimum Weight (kg)")]
        [Required(ErrorMessage = "Minimum Weight is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Minimum Weight must be greater than 0")]
        public double MinWeight { get; set; }

        [Display(Name = "Maximum Weight (kg)")]
        [Required(ErrorMessage = "Maximum Weight is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Maximum Weight must be greater than 0")]
        public double MaxWeight { get; set; }

        [Display(Name = "Base Fee")]
        [Required(ErrorMessage = "Base Fee is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Base Fee must be greater than 0")]
        public decimal BaseFee { get; set; }

        [Display(Name = "Extra Kilo Price")]
        [Required(ErrorMessage = "Extra Kilo Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Extra Kilo Price must be greater than 0")]
        public decimal ExtraKiloPrice { get; set; }

        [Display(Name = "Effective Date")]
        [Required(ErrorMessage = "Effective Date is required")]
        [DataType(DataType.DateTime)]
        public DateTime EffectiveDate { get; set; }

        [Display(Name = "Expiry Date")]
        [Required(ErrorMessage = "Expiry Date is required")]
        [DataType(DataType.DateTime)]
        public DateTime ExpiryDate { get; set; }
    }
}
