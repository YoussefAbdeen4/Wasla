using Wasla.Enums; 

namespace Wasla.Models
{
    public abstract class BasePhone
    {
        public int Id { get; set; }

       public string? PhoneNumber { get; set; }

        public bool IsPrimary { get; set; }

        public Label Label { get; set; } 

        public DateTime CreatedAt { get; set; }
    }
}