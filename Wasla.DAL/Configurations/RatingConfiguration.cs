using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class RatingConfiguration: IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(r => r.Id);
            builder
                .HasOne(r => r.Company)
                .WithMany(c => c.Ratings)
                .HasForeignKey(r => r.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Merchant)
                .WithMany(m => m.Ratings)
                .HasForeignKey(r => r.MerchantId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
