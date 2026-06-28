using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class ClientPhoneConfiguration : IEntityTypeConfiguration<ClientPhones>
    {
        public void Configure(EntityTypeBuilder<ClientPhones> builder)
        {
            builder.HasKey(x => x.ClientId);

            builder.HasOne(x => x.Client)
                .WithMany(x => x.Phones)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}