using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public  class RateCardConstraints:IEntityTypeConfiguration<RateCard>
    {
        public void Configure(EntityTypeBuilder<RateCard> builder)
        {
            // all properties are required except Id and companyId and navigatio property
            builder.Property(rc => rc.OriginCity).IsRequired();
            builder.Property(rc => rc.DestinationCity).IsRequired();
            builder.Property(rc => rc.MinWeight).IsRequired();
            builder.Property(rc => rc.MaxWeight).IsRequired();
            builder.Property(rc => rc.BaseFee).IsRequired();
            builder.Property(rc => rc.ExtraKiloPrice).IsRequired();
            builder.Property(rc => rc.EffectiveDate).IsRequired();
            builder.Property(rc => rc.ExpiryDate).IsRequired();


            // convert EgytCity to string in database
            builder.Property(rc => rc.OriginCity).HasConversion<string>().HasMaxLength(100);
            builder.Property(rc => rc.DestinationCity).HasConversion<string>().HasMaxLength(100);


            // set the precision of BaseFee and ExtraKiloPrice to 18,2
            builder.Property(rc => rc.BaseFee).HasPrecision(18, 4);
            builder.Property(rc => rc.ExtraKiloPrice).HasPrecision(18, 4);

            // check constraint to ensure that MinWeight is less than MaxWeight
            builder.ToTable(o=>o.HasCheckConstraint("CK_RateCard_MinWeight_MaxWeight", "[MinWeight] < [MaxWeight]"));

            // check constraint to ensure that EffectiveDate is less than ExpiryDate
            builder.ToTable(o=>o.HasCheckConstraint("CK_RateCard_EffectiveDate_ExpiryDate", "[EffectiveDate] < [ExpiryDate]"));

            // check constraint to ensure that BaseFee , MinWeight, and MaxWeight are greater than or equal to 0

            builder.ToTable(o=>o.HasCheckConstraint("CK_RateCard_BaseFee_NonNegative", "[BaseFee] >= 0"));
            builder.ToTable(o=>o.HasCheckConstraint("CK_RateCard_MinWeight_NonNegative", "[MinWeight] >= 0"));
            builder.ToTable(o=>o.HasCheckConstraint("CK_RateCard_MaxWeight_NonNegative", "[MaxWeight] >= 0"));



        }
    }
}
