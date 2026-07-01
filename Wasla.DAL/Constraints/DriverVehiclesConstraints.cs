
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class DriverVehiclesConstraints : IEntityTypeConfiguration<DriverVehicle>
    {
        public void Configure(EntityTypeBuilder<DriverVehicle> builder)
        {
            builder.Property(a => a.AssignedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            
        }

        
    }
}
