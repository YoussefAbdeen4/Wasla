using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class RatingConstraints : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            // all properties are required except Id and navigation properties
            builder.Property(r => r.Stars).IsRequired();
            builder.Property(r => r.Comment).IsRequired().HasMaxLength(500);
            builder.Property(r => r.CreatedAt).IsRequired();

            // convert Enums to string in database
            builder.Property(r => r.RatedBy).HasConversion<string>().HasMaxLength(100);

            // check constraint to ensure that Score is between 1 and 5
            builder.ToTable(o => o.HasCheckConstraint("CK_Rating_Score_Range", "[Stars] >= 1 AND [Stars] <= 5"));
        }
    }
}