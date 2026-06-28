using Wasla.Enums;

namespace Wasla.Models
{
    public class TrackingHistory
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public string Location { get; set; }
        public DateTime Timestamp { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
