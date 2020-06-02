using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class OrderItemReportConfiguration : IEntityTypeConfiguration<OrderItemReport>
    {
        public void Configure(EntityTypeBuilder<OrderItemReport> builder)
        {
            builder.HasKey(p => p.OrderItemId);

            builder.Property(p => p.OrderId)
                .HasColumnType("BIGINT")
                .IsRequired();
            builder.Property(p => p.OrderNumber)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.CustomerTypeId)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.OrderItemId)
                .HasColumnType("BIGINT")
                .IsRequired();
            builder.Property(p => p.OrderStatus)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
            builder.Property(p => p.Customer)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
            builder.Property(p => p.SocialIdentifier)
                .HasColumnType("VARCHAR(MAX)");

            builder.Property(p => p.ItemNumber)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.Product)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
            builder.Property(p => p.Quantity)
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();
            builder.Property(p => p.MeasureUnit)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
            builder.Property(p => p.CompleteBy)
                .HasColumnType("DATETIME")
                .IsRequired();
            builder.Property(p => p.LastStatusDate)
                .HasColumnType("DATETIME");
            builder.Property(p => p.OrderItemStatus)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
            builder.Property(p => p.FinalPrice)
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();
        }
    }
}
