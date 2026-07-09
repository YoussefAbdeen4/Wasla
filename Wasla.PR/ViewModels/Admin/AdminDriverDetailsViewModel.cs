namespace Wasla.PR.ViewModels.Admin
{
    public class AdminDriverDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public decimal TotalCashSubmitted { get; set; }
        public List<string> Phones { get; set; } = new();
        public string Status { get; set; }

        public int OrdersCount { get; set; }
        public int VehiclesCount { get; set; }
    }
}