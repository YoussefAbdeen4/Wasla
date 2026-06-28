using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class CourierCompanyConstraints : IEntityTypeConfiguration<CourierCompany>
    {
        public void Configure(EntityTypeBuilder<CourierCompany> builder)
        {
            builder.Property(c => c.CompanyName).IsRequired().HasMaxLength(100);
            builder.Property(c => c.CompanyEmail).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Password).IsRequired().HasMaxLength(100);
        }
    }
}