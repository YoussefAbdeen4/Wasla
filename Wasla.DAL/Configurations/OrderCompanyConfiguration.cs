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
            builder.HasOne(o => o.order).WithOne(oc => oc.orderCompany).HasForeignKey<OrderCompany>(o => o.OrderId);

            builder.HasOne(c => c.Company).WithMany(oc => oc.OrderCompanies).HasForeignKey(oc => oc.CompanyId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
