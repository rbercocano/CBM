using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class PersonCustomerConfiguration : IEntityTypeConfiguration<PersonCustomer>
    {
        public void Configure(EntityTypeBuilder<PersonCustomer> builder)
        {
            builder.HasBaseType<Customer>();
            builder.Property(p => p.Name)
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(100)");
            builder.Property(p => p.LastName)
                .HasColumnName("LastName")
                .HasColumnType("VARCHAR(100)");
            builder.Property(p => p.DateOfBirth)
                .HasColumnName("DateOfBirth")
                .HasColumnType("DATE");
        }
    }
}
