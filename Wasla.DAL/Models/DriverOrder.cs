
using Wasla.Enums;

namespace Wasla.Models
{
    public class DriverOrder
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public Driver Driver { get; set; }
        public int OrderId { get; set; }
        public IsActiveStatus  isActive{ get; set; }
        public Order Order { get; set; }
    }
}
