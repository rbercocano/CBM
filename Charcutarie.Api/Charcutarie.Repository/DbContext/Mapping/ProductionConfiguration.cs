using Charcutarie.Models.Entities;
using Charcutarie.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class ProductionConfiguration : IEntityTypeConfiguration<Production>
    {
        public void Configure(EntityTypeBuilder<Production> builder)
        {
            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.Product)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
            builder.Property(p => p.MeasureUnitId)
                .HasColumnType("INT")
                .IsRequired()
                 .HasConversion(v => (int)v,
                                v => (MeasureUnitEnum)v);
            builder.Property(p => p.MeasureUnitTypeId)
                .HasColumnType("INT")
                .IsRequired()
                 .HasConversion(v => (int)v,
                                v => (MeasureUnitTypeEnum)v);
            builder.Property(p => p.Quantity)
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();
        }
    }
}
