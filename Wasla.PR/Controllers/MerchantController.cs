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
         
    // افترضي إن دي أسماء الـ Properties اللي عندك في الموديل
    TotalOrders = 10,
    PendingOrders = 3,
    TotalSales = 5000
};
           

            return View(model); 
        }
    }
}
