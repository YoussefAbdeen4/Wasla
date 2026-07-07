using System.ComponentModel.DataAnnotations;
using Wasla.Enums;

public class EditDriverViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public VehicleType Vehicle { get; set; }

    [Required]
    public string LicensePlate { get; set; }
}