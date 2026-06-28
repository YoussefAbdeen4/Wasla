using Microsoft.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore.Metadata.Builders; 
using Wasla.Models; 

namespace Wasla.Constraints
{
    public abstract class BasePhoneConstraints<T> : IEntityTypeConfiguration<T> where T : BasePhone
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(15);
            builder.Property(p => p.IsPrimary).IsRequired();
            builder.Property(p => p.Label).HasConversion<string>().IsRequired().HasMaxLength(50);
            builder.Property(p => p.CreatedAt).IsRequired();
        }
    }
}