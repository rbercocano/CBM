using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class ContactTypeConfiguration : IEntityTypeConfiguration<ContactType>
    {
        public void Configure(EntityTypeBuilder<ContactType> builder)
        {
            builder.ToTable("ContactType");
            builder.HasKey(p => p.ContactTypeId);

            builder.Property(p => p.Description)
                .HasColumnType("VARCHAR(50)")
                .IsRequired();
            builder.Property(p => p.Icon)
                .HasColumnType("VARCHAR(20)")
                .IsRequired();


        }
    }
}
