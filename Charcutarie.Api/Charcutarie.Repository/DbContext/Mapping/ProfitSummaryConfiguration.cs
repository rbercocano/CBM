using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class ProfitSummaryConfiguration : IEntityTypeConfiguration<ProfitSummary>
    {
        public void Configure(EntityTypeBuilder<ProfitSummary> builder)
        {
            builder.HasKey(p => p.RowId);

            builder.Property(p => p.RowId)
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();
            builder.Property(p => p.TotalProfit)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.CurrentMonthProfit)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
