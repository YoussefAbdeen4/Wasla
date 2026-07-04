using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;
using System;

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

            builder.HasData(new Rating { Id = 1, Stars = 4.5, RatedBy = 0, Comment = "Good service", CreatedAt = new DateTime(2026, 6, 29, 12, 0, 0), CompanyId = 1, MerchantId = 1 }, new Rating { Id = 2, Stars = 5.0, RatedBy = 0, Comment = "Excellent", CreatedAt = new DateTime(2026, 6, 30, 12, 0, 0), CompanyId = 1, MerchantId = 2 }, new Rating { Id = 3, Stars = 3.5, RatedBy = 0, Comment = "Average", CreatedAt = new DateTime(2026, 7, 1, 12, 0, 0), CompanyId = 2, MerchantId = 3 });

        }
    }
}
