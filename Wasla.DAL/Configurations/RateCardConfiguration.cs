using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        }
    }
}
