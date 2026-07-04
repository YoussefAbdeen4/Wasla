using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class OrderConfiguration: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
           
            builder.HasKey(o => o.Id);

            builder.HasOne(m=>m.Merchant)
                .WithMany(m=>m.Orders)
                .HasForeignKey(o=>o.MerchantId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasData(new Order { Id = 1, CustomerName = "Mohamed Ali", CustomerPhone = "01001234567", TrackingUuid = "TRK1001", CustomerAddress = "Cairo", CityFrom = Wasla.Enums.EgyptCity.Cairo, CityTo = Wasla.Enums.EgyptCity.Giza, isClaimingRequired = false, isBreakable = false, TotalPrice = 120m, PaymentType = Wasla.Enums.PaymentType.COD, CreatedAt = new DateTime(2026, 7, 1, 12, 0, 0), UpdatedAt = new DateTime(2026, 7, 1, 12, 0, 0), DeliveredAt = default, MerchantId = 1, CompanyId = 1, DriverId = 1 }, new Order { Id = 2, CustomerName = "Nora Hussein", CustomerPhone = "01007654321", TrackingUuid = "TRK1002", CustomerAddress = "Giza", CityFrom = Wasla.Enums.EgyptCity.Giza, CityTo = Wasla.Enums.EgyptCity.Cairo, isClaimingRequired = false, isBreakable = true, TotalPrice = 80m, PaymentType = Wasla.Enums.PaymentType.Online, CreatedAt = new DateTime(2026, 6, 30, 12, 0, 0), UpdatedAt = new DateTime(2026, 7, 1, 12, 0, 0), DeliveredAt = default, MerchantId = 2, CompanyId = 1, DriverId = 1 }, new Order { Id = 3, CustomerName = "Omar Said", CustomerPhone = "01009998877", TrackingUuid = "TRK1003", CustomerAddress = "Alexandria", CityFrom = Wasla.Enums.EgyptCity.Cairo, CityTo = Wasla.Enums.EgyptCity.Alexandria, isClaimingRequired = true, isBreakable = false, TotalPrice = 200m, PaymentType = Wasla.Enums.PaymentType.COD, CreatedAt = new DateTime(2026, 6, 29, 12, 0, 0), UpdatedAt = new DateTime(2026, 7, 1, 12, 0, 0), DeliveredAt = default, MerchantId = 3, CompanyId = 1, DriverId = 1 });


        }
    }
}
