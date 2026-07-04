using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class CourierCompanyConfiguration : IEntityTypeConfiguration<CourierCompany>
    {
        public void Configure(EntityTypeBuilder<CourierCompany> builder)
        {
            builder.HasKey(t => t.Id);

            builder.ToTable("Courier Company");

            // seed sample companies
            builder.HasData(
                new CourierCompany { Id = 1, CompanyName = "Wasla Express", CompanyEmail = "info@wasla.com", Password = "pass123" },
                new CourierCompany { Id = 2, CompanyName = "FastShip", CompanyEmail = "hello@fastship.com", Password = "secret" },
                new CourierCompany { Id = 3, CompanyName = "CityCouriers", CompanyEmail = "contact@citycouriers.com", Password = "pwd123" }
            );
        }
    }
}