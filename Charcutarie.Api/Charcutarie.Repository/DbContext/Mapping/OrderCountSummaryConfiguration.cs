using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class OrderCountSummaryConfiguration : IEntityTypeConfiguration<OrderCountSummary>
    {
        public void Configure(EntityTypeBuilder<OrderCountSummary> builder)
        {
            builder.HasKey(p => p.RowId);

            builder.Property(p => p.RowId)
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();
            builder.Property(p => p.TotalOrders)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.TotalCompletedOrders)
                .HasColumnType("INT")
                .IsRequired();
        }
    }
}
