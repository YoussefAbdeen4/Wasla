namespace Wasla.PR.ViewModels.Merchant
{
    public class WalletViewModel
    {
        public decimal AvailableBalance { get; set; }
        public decimal PendingClearance { get; set; }
        public decimal TotalWithdrawn { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
