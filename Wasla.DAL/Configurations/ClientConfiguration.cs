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

            builder.HasData(
                new Client { Id = 1, Name = "Ahmed Client", Email = "ahmed.client@example.com", MerchantId = 1 },
                new Client { Id = 2, Name = "Layla Client", Email = "layla.client@example.com", MerchantId = 2 },
                new Client { Id = 3, Name = "Hassan Client", Email = "hassan.client@example.com", MerchantId = 3 }
            );
        }
    }
}