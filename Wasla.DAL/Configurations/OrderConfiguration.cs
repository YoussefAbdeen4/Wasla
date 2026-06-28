using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class OrderConfiguration: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
           
            builder.HasKey(o => o.Id);

            builder.HasOne(m=>m.Merchant)
                .WithMany(m=>m.Orders)
                .HasForeignKey(o=>o.MerchantId)
                .OnDelete(DeleteBehavior.Restrict);

         
            
        }
    }
}
