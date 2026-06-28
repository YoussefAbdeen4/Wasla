using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class DriverConfiguration: IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.HasKey(d => d.Id);
            
            builder.HasOne(c => c.Company)
                   .WithMany(d => d.Drivers)
                   .HasForeignKey(d => d.CompanyId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
