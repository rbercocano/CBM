using Charcutarie.Models.Entities;
using Charcutarie.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class ProductCostConfiguration : IEntityTypeConfiguration<VwProductCost>
    {
        public void Configure(EntityTypeBuilder<VwProductCost> builder)
        {
            builder.ToTable("VwProductCost");
            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.ProductId)
                .HasColumnType("BIGINT")
                .IsRequired();
            builder.Property(p => p.Cost)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.HasOne(p => p.Product)
                .WithOne(p => p.ProductCost);
        }
    }
}
