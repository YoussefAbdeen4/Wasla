namespace Wasla.PR.ViewModels.Admin
{
    public class AdminMerchantDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StoreName { get; set; }
        public string TaxNumber { get; set; }
        public string Category { get; set; }
        public decimal WalletBalance { get; set; }
        public List<string> Phones { get; set; } = new();
        public string Status { get; set; }

        public int OrdersCount { get; set; }
        public int RatingsCount { get; set; }
        public int ClientsCount { get; set; }
    }
}