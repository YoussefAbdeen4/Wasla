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
        }
    }
}
