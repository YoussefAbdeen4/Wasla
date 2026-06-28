

namespace Wasla.Models
{
    public class MerchantPhones : BasePhone
    {
        public int MerchantId { get; set; }
        public virtual Merchant Merchant { get; set; }
    }
}
