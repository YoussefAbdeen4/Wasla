using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class DriverOrderConfiguration: IEntityTypeConfiguration<DriverOrder>
    {
        public void Configure(EntityTypeBuilder<DriverOrder> builder)
        {
            builder.HasKey(d => d.Id);
            builder.ToTable("Driver-Orders");
            builder.HasOne(d => d.Driver)
                   .WithMany(dr => dr.DriverOrders)
                   .HasForeignKey(d => d.DriverId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Order)
                   .WithOne(o => o.DriverOrder)
                   .HasForeignKey<DriverOrder>(d => d.OrderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new DriverOrder { Id = 1, DriverId = 1, OrderId = 1, isActive = Wasla.Enums.IsActiveStatus.Active },
                new DriverOrder { Id = 2, DriverId = 2, OrderId = 2, isActive = Wasla.Enums.IsActiveStatus.Active },
                new DriverOrder { Id = 3, DriverId = 3, OrderId = 3, isActive = Wasla.Enums.IsActiveStatus.Active }
            );
        }
    }
}
