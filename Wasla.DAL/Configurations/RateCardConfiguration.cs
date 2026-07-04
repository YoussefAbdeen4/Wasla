using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Wasla.Enums;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class RateCardConfiguration: IEntityTypeConfiguration<RateCard>
    {
        public void Configure(EntityTypeBuilder<RateCard> builder)
        {
            builder.HasKey(rc => rc.Id);
            builder.ToTable("Rate Cards");
            builder
               .HasOne(rc => rc.Company)
               .WithMany(c => c.RateCards)
               .HasForeignKey(rc => rc.CompanyId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(new RateCard { Id = 1, OriginCity = EgyptCity.Cairo, DestinationCity = EgyptCity.Giza, MinWeight = 0, MaxWeight = 5, BaseFee = 50m, ExtraKiloPrice = 10m, EffectiveDate = new DateTime(2026, 6, 1, 12, 0, 0), ExpiryDate = new DateTime(2027, 7, 1, 12, 0, 0), CompanyId = 1 }, new RateCard { Id = 2, OriginCity = EgyptCity.Giza, DestinationCity = EgyptCity.Cairo, MinWeight = 0, MaxWeight = 10, BaseFee = 80m, ExtraKiloPrice = 8m, EffectiveDate = new DateTime(2026, 6, 21, 12, 0, 0), ExpiryDate = new DateTime(2027, 1, 1, 12, 0, 0), CompanyId = 2 }, new RateCard { Id = 3, OriginCity = EgyptCity.Cairo, DestinationCity = EgyptCity.Alexandria, MinWeight = 0, MaxWeight = 20, BaseFee = 150m, ExtraKiloPrice = 12m, EffectiveDate = new DateTime(2026, 6, 26, 12, 0, 0), ExpiryDate = new DateTime(2026, 10, 1, 12, 0, 0), CompanyId = 1 });
        }
    }
}
