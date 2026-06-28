using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class DriverVehiclesConfiguration: IEntityTypeConfiguration<DriverVehicle>
    {
        public void Configure(EntityTypeBuilder<DriverVehicle> builder)
        {
            builder.HasKey(dv => dv.Id);
            builder.ToTable("Driver-Vehicles");
            builder
               .HasOne(dv => dv.Driver)
               .WithMany(d => d.DriverVehicles)
               .HasForeignKey(dv => dv.DriverId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(dv => dv.Vehicle)
                .WithMany(v => v.DriverVehicles)
                .HasForeignKey(dv => dv.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}

