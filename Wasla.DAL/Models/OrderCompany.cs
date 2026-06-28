

namespace Wasla.Models
{
    public class OrderCompany
    {
        public int Id { get; set; }
        public decimal ShippingFee { get; set; }
        public bool AcceptanceState { get; set; }
        public int OrderId { get; set; }
        public Order order { get; set; }
        public int CompanyId { get; set; }
        public CourierCompany Company { get; set; }
    }
}
