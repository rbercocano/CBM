using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.HasKey(p => p.CustomerId);
            builder.HasDiscriminator(c => c.CustomerTypeId)
                .HasValue<PersonCustomer>(1)
                .HasValue<CompanyCustomer>(2);

            builder.Property(p => p.CustomerId).HasColumnType("BIGINT").UseIdentityColumn();

            builder.Property(p => p.CustomerTypeId)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.CorpClientId)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.CreatedOn)
                .HasColumnType("DateTimeOffset")
                .IsRequired();

            builder.Property(p => p.LastUpdated)
                .HasColumnType("DateTimeOffset");
        }
    }
}
