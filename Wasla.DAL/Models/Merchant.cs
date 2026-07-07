

using Wasla.DAL.Identity;

namespace Wasla.Models
{
    public class Merchant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StoreName { get; set; }
        public string TaxNumber { get; set; }
        public string Category { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        //public List<string> Phone { get; set; }
        public List<MerchantPhones> Phones { get; set; }
        public decimal WalletBalance { get; set; }

        public virtual List<Order> Orders { get; set; }
        public virtual List<Rating> Ratings { get; set; }
        public virtual List<Client> Clients { get; set; }

    }
}
