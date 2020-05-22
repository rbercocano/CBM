using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class CustomerContactConfiguration : IEntityTypeConfiguration<CustomerContact>
    {
        public void Configure(EntityTypeBuilder<CustomerContact> builder)
        {
            builder.ToTable("CustomerContact");
            builder.HasKey(p => p.CustomerContactId);
            builder.Property(p => p.CustomerContactId).UseIdentityColumn();

            builder.Property(p => p.ContactTypeId)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.Contact)
                .HasColumnType("VARCHAR(300)")
                .IsRequired();

            builder.Property(p => p.AdditionalInfo)
                .HasColumnType("VARCHAR(200)");

            builder.Property(p => p.CustomerId)
                .HasColumnType("BIGINT")
                .IsRequired();

        }
    }
}
