using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class SalesPerMonthConfiguration : IEntityTypeConfiguration<SalesPerMonth>
    {
        public void Configure(EntityTypeBuilder<SalesPerMonth> builder)
        {
            builder.HasKey(p => p.Date);

            builder.Property(p => p.Date)
                .HasColumnType("DATE")
                .IsRequired();
            builder.Property(p => p.TotalProfit)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.TotalSales)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
