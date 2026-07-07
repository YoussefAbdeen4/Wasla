namespace Wasla.PR.ViewModels.RateCards
{
    public class RateCardListViewModel
    {
        public int Id { get; set; }
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
        public double MinWeight { get; set; }
        public double MaxWeight { get; set; }
        public decimal BaseFee { get; set; }
        public decimal ExtraKiloPrice { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
