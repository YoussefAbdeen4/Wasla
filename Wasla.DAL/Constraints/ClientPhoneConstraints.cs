using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Models;

namespace Wasla.Constraints
{
    public class ClientPhoneConstraints : BasePhoneConstraints<ClientPhones>, IEntityTypeConfiguration<ClientPhones>
    {
        public override void Configure(EntityTypeBuilder<ClientPhones> builder)
        {
            base.Configure(builder);
        }
    }
}