using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class PendingPaymentsSummaryConfiguration : IEntityTypeConfiguration<PendingPaymentsSummary>
    {
        public void Configure(EntityTypeBuilder<PendingPaymentsSummary> builder)
        {
            builder.HasKey(p => p.RowId);

            builder.Property(p => p.RowId)
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();
            builder.Property(p => p.TotalPendingPayments)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.FinishedOrdersPendingPayment)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
