

namespace Wasla.Models
{
    public class CompanyPhones : BasePhone
    {
        public int CompanyId { get; set; }
        public virtual CourierCompany Company { get; set; }
    }
}
