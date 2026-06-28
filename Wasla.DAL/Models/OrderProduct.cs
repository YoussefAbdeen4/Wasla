

namespace Wasla.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
