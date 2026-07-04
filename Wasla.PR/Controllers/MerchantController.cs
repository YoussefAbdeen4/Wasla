using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Wasla.Models;
using Wasla.PR.ViewModels;
using Wasla; // تأكدي أن هذا هو الـ Namespace الصحيح لملف AppDbContext

namespace Wasla.Controllers
{
    public class MerchantController : Controller
    {
        private readonly AppDbContext _context;

        public MerchantController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // جلب البيانات الحقيقية من قاعدة البيانات باستخدام الـ _context
            var model = new MerchantDashboardViewModel
            {
                TotalOrders = _context.Orders.Count(),
                
                // حساب مجموع المبيعات مع التأكد من عدم وجود قيم فارغة
                TotalSales = _context.Orders.Sum(o => (decimal?)o.TotalPrice) ?? 0,
                
                // جلب آخر 5 طلبات تم إضافتها
                RecentOrders = _context.Orders
                                       .OrderByDescending(o => o.CreatedAt)
                                       .Take(5)
                                       .ToList()
            };

            return View(model);
        }
    }
}