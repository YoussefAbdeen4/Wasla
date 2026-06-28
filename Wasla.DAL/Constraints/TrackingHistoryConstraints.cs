using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class TrackingHistoryConstraints : IEntityTypeConfiguration<TrackingHistory>
    {
        public void Configure(EntityTypeBuilder<TrackingHistory> builder)
        {
            // all properties are required except Id and navigation properties
            builder.Property(th => th.Status).IsRequired();
            builder.Property(th => th.Location).IsRequired();

            // convert Enums to string in database
            builder.Property(th => th.Status).HasConversion<string>().HasMaxLength(100);
        }
    }
}
