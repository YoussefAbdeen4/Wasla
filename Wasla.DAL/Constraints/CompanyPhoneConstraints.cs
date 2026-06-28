using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class CompanyPhoneConstraints : BasePhoneConstraints<CompanyPhones>, IEntityTypeConfiguration<CompanyPhones>
    {
        public override void Configure(EntityTypeBuilder<CompanyPhones> builder)
        {
            base.Configure(builder);
        }
    }
}