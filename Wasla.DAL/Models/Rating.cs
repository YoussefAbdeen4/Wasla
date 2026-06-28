
using Wasla.Enums;

namespace Wasla.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public double Stars { get; set; }
        public RatedByWho RatedBy { get; set; }

        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CompanyId { get; set; }
        public virtual CourierCompany Company { get; set; }

        public int MerchantId { get; set; }
        public virtual Merchant Merchant { get; set; }
    }
}
