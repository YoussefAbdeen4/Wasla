using Wasla.Enums;

namespace Wasla.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public VehicleType Type { get; set; }  // enum
        public double Capacity { get; set; }
        public IsActiveStatus IsActive { get; set; }

        public int CompanyId { get; set; }
        public virtual CourierCompany Company { get; set; }
        //public int DriverId { get; set; }
        //public Driver Driver { get; set; } 

        public virtual List<DriverVehicle> DriverVehicles { get; set; }
    }
}
