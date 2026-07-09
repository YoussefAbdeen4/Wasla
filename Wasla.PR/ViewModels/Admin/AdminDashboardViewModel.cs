namespace Wasla.PR.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        public int TotalCompanies { get; set; }
        public int TotalMerchants { get; set; }
        public int TotalOrders { get; set; }
        public int TotalDrivers { get; set; }

        public List<AdminRecentOrderViewModel> RecentOrders { get; set; } = new();
        public List<AdminCompanyListItemViewModel> RecentCompanies { get; set; } = new();
    }

  

    
}