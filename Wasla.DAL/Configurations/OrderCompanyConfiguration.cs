using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class OrderCompanyConfiguration
    {
        public void Configure(EntityTypeBuilder<Models.OrderCompany> builder)
        {
            builder.HasKey(oc => oc.Id);

            builder.ToTable("Order-Companies");

            builder.HasOne(o => o.order)
                   .WithOne(oc => oc.orderCompany)
                   .HasForeignKey<OrderCompany>(o => o.OrderId);

            builder.HasOne(c => c.Company)
                   .WithMany(oc => oc.OrderCompanies)
                   .HasForeignKey(oc => oc.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasData(
                new OrderCompany { Id = 1, ShippingFee = 20m, AcceptanceState = true, OrderId = 1, CompanyId = 1 },
                new OrderCompany { Id = 2, ShippingFee = 15m, AcceptanceState = true, OrderId = 2, CompanyId = 2 },
                new OrderCompany { Id = 3, ShippingFee = 30m, AcceptanceState = false, OrderId = 3, CompanyId = 1 }
            );
        }
    }
}
