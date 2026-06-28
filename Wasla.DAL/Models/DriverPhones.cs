

namespace Wasla.Models
{
    public class DriverPhones : BasePhone
    {
        public int DriverId { get; set; }
        public virtual Driver Driver { get; set; }
    }
}
