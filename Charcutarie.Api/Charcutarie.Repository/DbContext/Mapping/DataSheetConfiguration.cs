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
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.ProcedureDescription)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();

            builder.HasOne(p => p.Product)
                .WithOne(p => p.DataSheet);
        }
    }
}
