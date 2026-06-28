using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class TrackingHistoryConfiguration: IEntityTypeConfiguration<TrackingHistory>
    {
        public void Configure(EntityTypeBuilder<TrackingHistory> builder)
        {
           builder.HasKey(th => th.Id);
            builder.ToTable("Tracking Histories of Orders");
            builder
                .HasOne(th => th.Order)
                .WithMany(o => o.TrackingHistories)
                .HasForeignKey(th => th.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
