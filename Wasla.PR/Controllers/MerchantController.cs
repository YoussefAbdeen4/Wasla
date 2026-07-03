using Microsoft.AspNetCore.Mvc;

namespace Wasla.Controllers
{
    public class MerchantController : Controller
    {
        // غيرنا الاسم لـ Index عشان يطابق اسم ملف الـ cshtml اللي عندك
        public IActionResult Index()
        {
            return View(); 
        }
    }
}
