using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class OrderProductConfiguration: IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.HasKey(op => op.Id);
             builder.ToTable("Order-Products");
            builder
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new OrderProduct { Id = 1, Name = "Item A", Qty = 1, Price = 50m, OrderId = 1 },
                new OrderProduct { Id = 2, Name = "Item B", Qty = 2, Price = 35m, OrderId = 2 },
                new OrderProduct { Id = 3, Name = "Item C", Qty = 1, Price = 200m, OrderId = 3 }
            );


           
        }
    }
}
