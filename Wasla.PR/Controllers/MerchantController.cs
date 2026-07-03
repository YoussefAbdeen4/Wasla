using Microsoft.AspNetCore.Mvc;

namespace Wasla.Controllers
{
    public class MerchantController : Controller
    {
        // ده الـ Action اللي هيفتح صفحة الداش بورد
        public IActionResult Dashboard()
        {
            return View(); 
        }
    }
}
