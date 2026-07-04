using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class MerchantConfiguration: IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
           
         builder.HasKey(m => m.Id);

          builder.HasData(
              new Merchant { Id = 1, Name = "Alfa Store", Email = "alfa@store.com", Password = "m1npass", StoreName = "Alfa Store", WalletBalance = 1000m },
              new Merchant { Id = 2, Name = "Beta Shop", Email = "beta@shop.com", Password = "m2npass", StoreName = "Beta Shop", WalletBalance = 250m },
              new Merchant { Id = 3, Name = "Gamma Market", Email = "gamma@market.com", Password = "m3npass", StoreName = "Gamma Market", WalletBalance = 500m }
          );
        }
    }
}
