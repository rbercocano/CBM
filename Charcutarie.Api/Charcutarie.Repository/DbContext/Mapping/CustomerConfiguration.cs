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


            //builder.Property(p => p.Name)
            //    .HasColumnName("Name")
            //    .HasColumnType("VARCHAR(100)");
            //builder.Property(p => p.LastName)
            //    .HasColumnName("LastName")
            //    .HasColumnType("VARCHAR(100)");
            //builder.Property(p => p.DateOfBirth)
            //    .HasColumnName("DateOfBirth")
            //    .HasColumnType("DATETIME");

            //builder.Property(p => p.DBAName)
            //    .HasColumnName("DBANAME")
            //    .HasColumnType("VARCHAR(100)");
            //builder.Property(p => p.Cnpj)
            //    .HasColumnName("CNPJ")
            //    .HasColumnType("VARCHAR(18)");

        }
    }
}
