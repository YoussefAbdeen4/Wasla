using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class DriverPhoneConfiguration : IEntityTypeConfiguration<DriverPhones>
    {
        public void Configure(EntityTypeBuilder<DriverPhones> builder)
        {
            builder.HasKey(x => x.DriverId);

            builder.HasOne(x => x.Driver)
                .WithMany(x => x.Phones)
                .HasForeignKey(x => x.DriverId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
