using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class CompanyPhoneConfiguration : IEntityTypeConfiguration<CompanyPhones>
    {
        public void Configure(EntityTypeBuilder<CompanyPhones> builder)
        {
            builder.HasKey(x => x.CompanyId);

            builder.HasOne(x => x.Company)
                .WithMany(x => x.Phones)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}