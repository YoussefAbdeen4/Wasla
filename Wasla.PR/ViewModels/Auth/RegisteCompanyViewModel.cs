using System.ComponentModel.DataAnnotations;
using Wasla.Enums;

namespace Wasla.PR.ViewModels.Auth
{
    public class RegisteCompanyViewModel
    {
        // Account Information
        [Required(ErrorMessage = "Please select account type.")]
        public UserType UserType { get; set; } = UserType.Company;

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Tax Number is required.")]
        public string TaxNumber { get; set; }

    }
}
