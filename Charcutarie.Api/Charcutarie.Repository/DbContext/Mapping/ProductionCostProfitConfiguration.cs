using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class ProductionCostProfitConfiguration : IEntityTypeConfiguration<ProductionCostProfit>
    {
        public void Configure(EntityTypeBuilder<ProductionCostProfit> builder)
        {
            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.Product)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
            builder.Property(p => p.Cost)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.Profit)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.ProfitPercentage)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
