using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class DataSheetItemConfiguration : IEntityTypeConfiguration<DataSheetItem>
    {
        public void Configure(EntityTypeBuilder<DataSheetItem> builder)
        {
            builder.ToTable("DataSheetItem");
            builder.HasKey(p => p.DataSheetItemId);

            builder.Property(p => p.Percentage)
                .HasColumnType("decimal(9,5)")
                .IsRequired();
            builder.Property(p => p.DataSheetId)
                .HasColumnType("BIGINT")
                .IsRequired();
            builder.Property(p => p.RawMaterialId)
                .HasColumnType("BIGINT")
                .IsRequired();
            builder.Property(p => p.IsBaseItem)
                .HasColumnType("BIT")
                .IsRequired();
        }
    }
}
