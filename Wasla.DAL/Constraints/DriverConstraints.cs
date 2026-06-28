using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class DriverConstraints : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Email).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Password).IsRequired().HasMaxLength(100);
        }
    }
}