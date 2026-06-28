using Wasla.Enums;

namespace Wasla.Models
{
    public class RateCard
    {
        public int Id { get; set; }
        public EgyptCity OriginCity { get; set; }
        public EgyptCity DestinationCity { get; set; }
        public double MinWeight { get; set; }
        public double MaxWeight { get; set; }
        public decimal BaseFee { get; set; }
        public decimal ExtraKiloPrice { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public int CompanyId { get; set; }
        public virtual CourierCompany Company { get; set; }
    }
}
