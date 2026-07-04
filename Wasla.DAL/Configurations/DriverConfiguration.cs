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

            // seed sample drivers
            builder.HasData(
                new Driver { Id = 1, Name = "Ziad Ahmed", Email = "ziad@example.com", Password = "drv1", CompanyId = 1 },
                new Driver { Id = 2, Name = "Omar Khalid", Email = "omar@example.com", Password = "drv2", CompanyId = 2 },
                new Driver { Id = 3, Name = "Sara Ali", Email = "sara@example.com", Password = "drv3", CompanyId = 1 }
            );

        }
    }
}
