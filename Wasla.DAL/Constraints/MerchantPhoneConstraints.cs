using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class MerchantPhoneConstraints : BasePhoneConstraints<MerchantPhones>, IEntityTypeConfiguration<MerchantPhones>
    {
        public override void Configure(EntityTypeBuilder<MerchantPhones> builder)
        {
            base.Configure(builder);
        }
    }
}
