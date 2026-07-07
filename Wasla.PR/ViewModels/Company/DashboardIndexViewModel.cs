using Wasla.Enums;

namespace Wasla.PR.ViewModels.Company
{
    public class DashboardIndexViewModel
    {
        public DashboardStatisticsViewModel Statistics { get; set; }
        public List<RecentOrderViewModel> RecentOrders { get; set; } = new();
        public List<AvailableDriverViewModel> AvailableDrivers { get; set; } = new();
    }

    public class DashboardStatisticsViewModel
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int OutForDeliveryOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int FailedOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalDrivers { get; set; }
        public int TotalAgents { get; set; }
    }

    public class RecentOrderViewModel
    {
        public int Id { get; set; }
        public string TrackingUuid { get; set; }
        public string CustomerName { get; set; }
        public string DriverName { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AvailableDriverViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int ActiveOrders { get; set; }
    }
}
