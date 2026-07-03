using Microsoft.AspNetCore.Mvc;
using Wasla.Models; // اتأكدي إنك عاملة Using للـ Namespace بتاع الموديلز عندك

namespace Wasla.Controllers
{
    public class MerchantController : Controller
    {
        public IActionResult Index()
        {
            // هنا بنجهز البيانات اللي هنبعتها للـ View
         var model = new MerchantDashboardViewModel
{
    TotalOrders = 10,
    PendingOrders = 3,
    TotalSales = 5000,
    // ضيف السطر ده:
   RecentOrders = new List<Order>
{
    new Order { Id = 1, CustomerName = "أحمد محمد", Amount = 150, Status = "مكتمل", Date = DateTime.Now },
    new Order { Id = 2, CustomerName = "سارة علي", Amount = 200, Status = "قيد التنفيذ", Date = DateTime.Now }
};
           

            return View(model); 
        }
    }
}
