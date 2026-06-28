using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Merchant)
                .WithMany(m => m.Clients)
                .HasForeignKey(c => c.MerchantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}