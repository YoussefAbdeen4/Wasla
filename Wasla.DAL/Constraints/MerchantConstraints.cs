using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class MerchantConstraints : IEntityTypeConfiguration<Merchant>   
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
            builder.Property(m => m.Email).IsRequired().HasMaxLength(100);
            builder.Property(m => m.Password).IsRequired().HasMaxLength(100);

            // make properties with data types numbers non negative
            builder.ToTable(t => t.HasCheckConstraint("CK_Product_WalletBalance_NonNegative", "[WalletBalance] >= 0"));

            // make walletbalance has precision 18,2
            builder.Property(t => t.WalletBalance).HasPrecision(18, 4);
        
        }
    }
}
