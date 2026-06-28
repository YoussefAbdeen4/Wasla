
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;
namespace Wasla.Constraints
{
    public class DriverOrderConstraints : IEntityTypeConfiguration<DriverOrder>
    {
        public void Configure(EntityTypeBuilder<DriverOrder> builder)
        {
            builder.Property(e => e.isActive)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // convert isActive to string and set max length to 20
             builder.Property(e => e.isActive)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
