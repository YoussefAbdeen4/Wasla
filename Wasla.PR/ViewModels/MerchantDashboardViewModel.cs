using System;
using System.Collections.Generic;
using Wasla.Models;

namespace Wasla.PR.ViewModels
{
    public class MerchantDashboardViewModel
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public decimal TotalSales { get; set; }
        
        // الخصائص اللي كانت عاملة إيرور في الـ View
        public int RejectedOrdersCount { get; set; }
        public int UnderReviewOrdersCount { get; set; }
        public int TotalOrdersCount { get; set; }
        public int DeliveredOrdersCount { get; set; }
        
       public List<Order>? RecentOrders { get; set; }
    }
}