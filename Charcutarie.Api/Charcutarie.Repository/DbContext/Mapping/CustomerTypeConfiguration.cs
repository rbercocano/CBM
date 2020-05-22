using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class CustomerTypeConfiguration : IEntityTypeConfiguration<CustomerType>
    {
        public void Configure(EntityTypeBuilder<CustomerType> builder)
        {
            builder.ToTable("CustomerType");
            builder.HasKey(p => p.CustomerTypeId);

            builder.Property(p => p.Description)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();


        }
    }
}
