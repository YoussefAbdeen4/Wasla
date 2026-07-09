namespace Wasla.PR.ViewModels.Admin
{
    public class AdminOrdersListViewModel
    {
        public List<AdminOrderListItemViewModel> Orders { get; set; } = new();
    }

    public class AdminOrderListItemViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}