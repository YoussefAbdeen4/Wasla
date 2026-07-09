namespace Wasla.PR.ViewModels.Admin
{
    public class AdminOrderDetailsViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public string MerchantStoreName { get; set; }
        public string CompanyName { get; set; }
        public string DriverName { get; set; }

        public int ProductsCount { get; set; }
    }
}