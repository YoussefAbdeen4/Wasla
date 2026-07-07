using System.ComponentModel.DataAnnotations;

namespace Wasla.PR.ViewModels.Company
{
    public class CompanyProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الشركة مطلوب")]
        [StringLength(200, ErrorMessage = "اسم الشركة لا يجب أن يتجاوز 200 حرف")]
        [Display(Name = "اسم الشركة")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "اسم المسؤول مطلوب")]
        [StringLength(200, ErrorMessage = "اسم المسؤول لا يجب أن يتجاوز 200 حرف")]
        [Display(Name = "اسم المسؤول")]
        public string Name { get; set; }

        [Required(ErrorMessage = "رقم الضريبة مطلوب")]
        [StringLength(50, ErrorMessage = "رقم الضريبة لا يجب أن يتجاوز 50 حرف")]
        [Display(Name = "رقم الضريبة")]
        public string TaxNumber { get; set; }

        [Display(Name = "صورة الملف الشخصي")]
        public IFormFile? ProfileImage { get; set; }

        public string? ExistingImagePath { get; set; }

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }

        public List<string> DeliveryZones { get; set; } = new();
    }
}
