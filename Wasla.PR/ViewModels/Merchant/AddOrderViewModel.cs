using System;
using System.ComponentModel.DataAnnotations;
using Wasla.Enums;

namespace Wasla.PR.ViewModels.Merchant
{
    public class AddOrderViewModel
    {
        [Required(ErrorMessage = "اسم العميل مطلوب")]
        [MaxLength(100, ErrorMessage = "الاسم يجب ألا يتجاوز 100 حرف")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "رقم الهاتف يجب أن يتكون من 11 رقماً")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "يجب أن يبدأ رقم الهاتف بـ 01")]
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
