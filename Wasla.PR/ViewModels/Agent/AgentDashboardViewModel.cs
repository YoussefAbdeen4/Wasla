using System.Collections.Generic;

namespace Wasla.PR.ViewModels.Agent
{
    public class AgentDashboardViewModel
    {
        public int TotalOrders { get; set; }
        public int Delivered { get; set; }
        public int Returned { get; set; }

        public decimal CashInHand { get; set; }
        public decimal CashDelivered { get; set; }
        public decimal Balance { get; set; }

        public List<OrderSummaryViewModel> RecentOrders { get; set; } = new List<OrderSummaryViewModel>();
    }
}
