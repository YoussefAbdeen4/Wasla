namespace Wasla.PR.ViewModels.Agent
{
    public class AgentDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }

        public string VehicleType { get; set; }
        public string PlateNumber { get; set; }

        public string Status { get; set; }
        public string Avatar { get; set; }

        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int ReturnedOrders { get; set; }
    }
}
