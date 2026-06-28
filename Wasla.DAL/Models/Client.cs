

namespace Wasla.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        //public List<string> Phones { get; set; }
        public List<ClientPhones> Phones { get; set; }
        public int MerchantId { get; set; }
        public Merchant Merchant { get; set; }
    }
}
