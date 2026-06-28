using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Configurations
{
    public class CourierCompanyConfiguration : IEntityTypeConfiguration<CourierCompany>
    {
        public void Configure(EntityTypeBuilder<CourierCompany> builder)
        {
            builder.HasKey(t => t.Id);

            builder.ToTable("Courier Company");
        }
    }
}