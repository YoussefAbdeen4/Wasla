using System;

namespace Wasla.PR.ViewModels.Agent
{
    public class AgentOrderViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        // Additional fields useful for actions
        public bool CanPickUp { get; set; }
        public bool CanDeliver { get; set; }
        public bool CanReturn { get; set; }
    }
}
