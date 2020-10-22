using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class SalesSummaryConfiguration : IEntityTypeConfiguration<SalesSummary>
    {
        public void Configure(EntityTypeBuilder<SalesSummary> builder)
        {
            builder.HasKey(p => p.RowId);

            builder.Property(p => p.RowId)
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();
            builder.Property(p => p.TotalSales)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.CurrentMonthSales)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
