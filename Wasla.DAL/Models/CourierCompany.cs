
using Wasla.DAL.Identity;

namespace Wasla.Models
{
    public class CourierCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        //public List<string> Phone { get; set; }
        public List<CompanyPhones> Phones { get; set; }

        // Navigation Properties
        public virtual List<Driver> Drivers { get; set; }
        public virtual List<Vehicle> Vehicles { get; set; }
        public virtual List<RateCard> RateCards { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual List<Rating> Ratings { get; set; }
        public virtual List<OrderCompany> OrderCompanies { get; set; }

    }
}
