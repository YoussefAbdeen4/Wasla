

namespace Wasla.Models
{
    public class Client
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    
    // ضيفي علامة الاستفهام هنا عشان الـ List
    public List<ClientPhones>? Phones { get; set; } 
    
    public int MerchantId { get; set; }
    
    // ضيفي علامة الاستفهام هنا لأن الـ Merchant ده كلاس
    public Merchant? Merchant { get; set; } 
}}