namespace Wasla.PR.ViewModels.Admin
{
    public class AdminRecentOrderViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}