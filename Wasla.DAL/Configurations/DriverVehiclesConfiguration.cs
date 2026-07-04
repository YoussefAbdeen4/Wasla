using Microsoft.EntityFrameworkCore;
using System;
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
            builder.HasData(new DriverVehicle { Id = 1, AssignedAt = new DateTime(2026, 6, 21, 12, 0, 0), ReturnedAt = DateTime.MinValue, DriverId = 1, VehicleId = 1 }, new DriverVehicle { Id = 2, AssignedAt = new DateTime(2026, 6, 26, 12, 0, 0), ReturnedAt = DateTime.MinValue, DriverId = 2, VehicleId = 2 }, new DriverVehicle { Id = 3, AssignedAt = new DateTime(2026, 6, 30, 12, 0, 0), ReturnedAt = DateTime.MinValue, DriverId = 3, VehicleId = 3 });


        }
    }
}

