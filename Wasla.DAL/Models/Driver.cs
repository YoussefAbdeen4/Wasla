

namespace Wasla.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal TotalCashSubmitted { get; set; }

        //public List<string> Phone { get; set; }
        public List<DriverPhones> Phones { get; set; }
        public int CompanyId { get; set; }
        public virtual CourierCompany Company { get; set; }

        public virtual List<Order> Orders { get; set; }
        public virtual List<DriverVehicle> DriverVehicles { get; set; }
        public virtual List<DriverOrder> DriverOrders { get; set; }

    }
}
