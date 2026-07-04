using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class VehicleConfiguration: IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
           builder.HasKey(v => v.Id);
             builder
                 .HasOne(v => v.Company)
                 .WithMany(c => c.Vehicles)
                 .HasForeignKey(v => v.CompanyId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Vehicle { Id = 1, LicensePlate = "ABC-123", Type = Wasla.Enums.VehicleType.Car, Capacity = 500, IsActive = Wasla.Enums.IsActiveStatus.Active, CompanyId = 1,DriverId = 1 },
                new Vehicle { Id = 2, LicensePlate = "XYZ-987", Type = Wasla.Enums.VehicleType.Motorcycle, Capacity = 50, IsActive = Wasla.Enums.IsActiveStatus.Active, CompanyId = 2,DriverId = 1 },
                new Vehicle { Id = 3, LicensePlate = "LMN-456", Type = Wasla.Enums.VehicleType.Van, Capacity = 1200, IsActive = Wasla.Enums.IsActiveStatus.Active, CompanyId = 1,DriverId = 1 }
            );
        }
    }
}
