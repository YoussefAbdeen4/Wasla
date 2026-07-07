using Wasla.Models;

namespace Wasla.PR.ViewModels.Company
{
    public class AllOrdersViewModel
    {
        public int TotalOrders { get; set; }

        public int PendingOrders { get; set; }

        public int OutForDeliveryOrders { get; set; }

        public int DeliveredOrders { get; set; }

        public int ReturnedOrders { get; set; }

        public int WaitingApprovalOrders { get; set; }

        public List<OrderItemViewModel> LatestOrders { get; set; } = new();
    }
}
