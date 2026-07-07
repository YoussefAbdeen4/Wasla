using Wasla.Enums;

namespace Wasla.PR.ViewModels.Company
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; }

        public string CustomerName { get; set; }

        public string DriverName { get; set; }

        public decimal Amount { get; set; }

        public OrderStatus Status { get; set; }
    }
}