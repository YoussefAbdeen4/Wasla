

namespace Wasla.Models
{
    public class ClientPhones : BasePhone
    {
        public int ClientId { get; set; }
        public virtual Client? Client { get; set; }
    }
}
