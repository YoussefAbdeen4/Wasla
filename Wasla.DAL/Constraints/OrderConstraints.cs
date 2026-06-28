using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class OrderConstraints : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
      
            builder.Property(o => o.CustomerName).IsRequired().HasMaxLength(100);
            builder.Property(o => o.CustomerPhone).IsRequired().HasMaxLength(100);
            builder.Property(o => o.TrackingUuid).IsRequired().HasMaxLength(100);
            builder.Property(o => o.CustomerAddress).IsRequired();
            builder.Property(o => o.CityFrom).IsRequired();
            builder.Property(o => o.CityTo).IsRequired();
            builder.Property(o => o.isClaimingRequired).IsRequired();
            builder.Property(o => o.isBreakable).IsRequired();
            builder.Property(o => o.PaymentType).IsRequired();
            builder.Property(o => o.CreatedAt).IsRequired();


            // convert Enums to string 
            builder.Property(o=>o.CityFrom).HasConversion<string>().HasMaxLength(100);
            builder.Property(o=>o.CityTo).HasConversion<string>().HasMaxLength(100);
            builder.Property(o=>o.PaymentType).HasConversion<string>().HasMaxLength(100);


            // make check constraint to ensure that total price is greater than or equal to zero
            builder.ToTable(o => o.HasCheckConstraint("CK_Order_TotalPrice", "TotalPrice >= 0"));
            builder.Property(t => t.TotalPrice).HasPrecision(18, 4);

            // change phone from string to char 11
            builder.Property(o => o.CustomerPhone).HasColumnType("char(11)");

        }
    }
}
