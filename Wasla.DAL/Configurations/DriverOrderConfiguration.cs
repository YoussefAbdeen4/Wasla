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
        }
    }
}
