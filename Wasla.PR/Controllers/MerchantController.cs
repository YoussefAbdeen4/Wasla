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
                // دي بيانات تجريبية مؤقتاً لحد ما نربط الـ Database
                // عدلي الأسماء حسب البروبرتيز اللي إنتِ معرفاها في الموديل
            };

            return View(model); 
        }
    }
}
