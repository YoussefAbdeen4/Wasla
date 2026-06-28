using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class VehicleConstraints : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
           // make license plate and type and capacity required and make the string property is max length 100
            builder.Property(v => v.LicensePlate).IsRequired().HasMaxLength(100);
            builder.Property(v => v.Type).IsRequired();
            builder.Property(v => v.Capacity).IsRequired();

            // convert Enums to string with max length 100 convert vehicte type to string and is active status to string
            builder.Property(v => v.Type).HasConversion<string>().HasMaxLength(100);
            builder.Property(v => v.IsActive).HasConversion<string>().HasMaxLength(100);

        }
    }
}
