using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class DriverPhoneConstraints : BasePhoneConstraints<DriverPhones>, IEntityTypeConfiguration<DriverPhones>
    {
        public override void Configure(EntityTypeBuilder<DriverPhones> builder)
        {
            base.Configure(builder);
        }
    }
}
