using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class DataSheetConfiguration : IEntityTypeConfiguration<DataSheet>
    {
        public void Configure(EntityTypeBuilder<DataSheet> builder)
        {
            builder.ToTable("DataSheet");
            builder.HasKey(p => p.DataSheetId);
            builder.HasIndex(p => p.ProductId).IsUnique();
            builder.Property(p => p.ProductId)
                .HasColumnType("BIGINT")
                .IsRequired();

            builder.Property(p => p.WeightVariationPercentage)
                .HasColumnType("DECIMAL(4,2)")
                .IsRequired();

            builder.Property(p => p.IncreaseWeight)
                .HasColumnType("BIT")
                .IsRequired();

            builder.Property(p => p.ProcedureDescription)
                .HasColumnType("VARCHAR(MAX)");

            builder.HasOne(p => p.Product)
                .WithOne(p => p.DataSheet);
        }
    }
}
