using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class OrderCompanyConstraints : IEntityTypeConfiguration<OrderCompany>
    {
        public void Configure(EntityTypeBuilder<OrderCompany> builder)
        {
            builder.ToTable(t => t.HasCheckConstraint("CK_OrderCompany_ShippingFee_NonNegative", "[ShippingFee] >= 0"));
            builder.Property(t => t.ShippingFee).HasPrecision(18, 4);
        }
    }
}
