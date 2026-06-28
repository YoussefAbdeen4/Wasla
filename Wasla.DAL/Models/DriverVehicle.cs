

namespace Wasla.Models
{
    public class DriverVehicle
    {
        public int Id { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime ReturnedAt { get; set; }


        public int DriverId { get; set; }
        public virtual Driver Driver { get; set; }
        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
