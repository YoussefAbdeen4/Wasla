
namespace Wasla.Enums
{
    public enum OrderStatus
    {
        none = 0,
        Draft = 1,                 // مسودة
        Created = 2,               // تم الإنشاء
        PendingConfirmation = 3,   // قيد التأكيد
        Packed = 4,                // تم التغليف
        ReadyForPickup = 5,        // جاهز للاستلام
        PickedUp = 6,              // تم الاستلام من التاجر
        InTransit = 7,             // في الطريق (بين المحافظات)
        OutForDelivery = 8,        // خرج للتوصيل
        Delivered = 9,             // تم التسليم
        FailedDelivery = 10,       // فشل التوصيل
        Cancelled = 11,            // ملغي
        Returned = 12              // تم إرجاعه إلى التاجر
    }
}
