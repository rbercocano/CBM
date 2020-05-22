using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class CompanyCustomerConfiguration : IEntityTypeConfiguration<CompanyCustomer>
    {
        public void Configure(EntityTypeBuilder<CompanyCustomer> builder)
        {
            builder.Property(p => p.Name)
                .HasColumnName("NAME")
                .HasColumnType("VARCHAR(100)");
            builder.Property(p => p.DBAName)
                .HasColumnName("DBANAME")
                .HasColumnType("VARCHAR(100)");
        }
    }
}
