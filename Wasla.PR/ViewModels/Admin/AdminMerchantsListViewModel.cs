namespace Wasla.PR.ViewModels.Admin
{
    public class AdminMerchantsListViewModel
    {
        public List<AdminMerchantListItemViewModel> Merchants { get; set; } = new();
    }

    public class AdminMerchantListItemViewModel
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string Phone { get; set; }
        public int OrdersCount { get; set; }
        public string Status { get; set; }
    }
}