using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(p => p.ProductId);
            builder.Property(p => p.ProductId)
                .HasColumnType("BIGINT")
                .UseIdentityColumn();
            builder.Property(p => p.Name)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(p => p.MeasureUnitId)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired();

            builder.Property(p => p.ActiveForSale)
                .HasColumnType("BIT")
                .IsRequired();

            builder.Property(p => p.CorpClientId)
                .HasColumnType("INT")
                .IsRequired();

        }
    }
}
