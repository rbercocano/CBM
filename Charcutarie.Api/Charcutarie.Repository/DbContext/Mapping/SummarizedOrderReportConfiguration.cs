using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class SummarizedOrderReportConfiguration : IEntityTypeConfiguration<SummarizedOrderReport>
    {
        public void Configure(EntityTypeBuilder<SummarizedOrderReport> builder)
        {
            builder.HasKey(p => p.RowId);

            builder.Property(p => p.RowId)
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();
            builder.Property(p => p.Quantity)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.MeasureUnitTypeId)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.MeasureUnitId)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.MeasureUnit)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
            builder.Property(p => p.ShortMeasureUnit)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
            builder.Property(p => p.Product)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
            builder.Property(p => p.ProductId)
                .HasColumnType("BIGINT")
                .IsRequired();
            builder.Property(p => p.OrderItemStatusId)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.OrderItemStatus)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
        }
    }
}
