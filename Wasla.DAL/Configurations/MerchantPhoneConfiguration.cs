using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class MerchantPhoneConfiguration : IEntityTypeConfiguration<MerchantPhones>
    {
        public void Configure(EntityTypeBuilder<MerchantPhones> builder)
        {
            builder.HasKey(x => x.MerchantId);

            builder.HasOne(x => x.Merchant)
                .WithMany(x => x.Phones)
                .HasForeignKey(x => x.MerchantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
