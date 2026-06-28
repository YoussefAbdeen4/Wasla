using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class OrderProductConstraints : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.Property(op => op.Name).IsRequired().HasMaxLength(100);
            builder.Property(op => op.Price).IsRequired();
            builder.Property(op => op.Qty).IsRequired();

            // make check constraint to ensure that quantity is greater than or equal to zero
            builder.ToTable(op => op.HasCheckConstraint("CK_OrderProduct_Quantity", "QTY >= 0"));

            // make check constraint to ensure that price is greater than or equal to zero
            builder.ToTable(op => op.HasCheckConstraint("CK_OrderProduct_Price", "Price >= 0"));
            builder.Property(t => t.Price).HasPrecision(18, 4);
        }
    }
}