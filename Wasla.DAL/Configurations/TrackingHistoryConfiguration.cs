using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;
using System;

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

            builder.HasData(new TrackingHistory { Id = 1, OrderId = 1, Status = Wasla.Enums.OrderStatus.ReadyForPickup, Location = "Merchant", Timestamp = new DateTime(2026, 6, 29, 12, 0, 0) }, new TrackingHistory { Id = 2, OrderId = 2, Status = Wasla.Enums.OrderStatus.Packed, Location = "Warehouse", Timestamp = new DateTime(2026, 6, 30, 12, 0, 0) }, new TrackingHistory { Id = 3, OrderId = 3, Status = Wasla.Enums.OrderStatus.Created, Location = "System", Timestamp = new DateTime(2026, 7, 1, 12, 0, 0) });

        }
    }
}
