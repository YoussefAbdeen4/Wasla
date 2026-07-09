using System;
namespace Wasla.PR.ViewModels.Merchant
{
    public class OrderPreviewViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
