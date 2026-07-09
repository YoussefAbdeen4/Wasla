using System.ComponentModel.DataAnnotations;

namespace Wasla.PR.ViewModels.Merchant
{
    public class MerchantProfileViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string StoreName { get; set; }

        public string TaxNumber { get; set; }

        public string Category { get; set; }
    }
}
