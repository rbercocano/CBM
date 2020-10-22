using Charcutarie.Models.Entities;
using Charcutarie.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItem");
            builder.HasKey(p => p.OrderItemId);

            builder.Property(p => p.OrderId)
                .HasColumnType("BIGINT")
                .IsRequired();
            builder.Property(p => p.CreatedOn)
                .HasColumnType("DATETIME")
                .IsRequired();
            builder.Property(p => p.LastUpdated)
                .HasColumnType("DATETIME");
            builder.Property(p => p.ProductId)
                .HasColumnType("BIGINT")
                .IsRequired();
            builder.Property(p => p.Quantity)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.OrderItemStatusId)
                .HasColumnType("INT")
                .IsRequired()
                 .HasConversion(v => (int)v,
                                v => (OrderItemStatusEnum)v);
            builder.Property(p => p.MeasureUnitId)
                .HasColumnType("INT")
                .IsRequired()
                 .HasConversion(v => (int)v,
                                v => (MeasureUnitEnum)v);
            builder.Property(p => p.AdditionalInfo)
                .HasColumnType("VARCHAR(200)")
                .IsRequired();
            builder.Property(p => p.OriginalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.Discount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.PriceAfterDiscount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.ProductPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.ItemNumber)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.Cost)
                .HasColumnType("decimal(18,2)");
            builder.Property(p => p.Profit)
                .HasColumnType("decimal(18,2)");
            builder.Property(p => p.ProfitPercentage)
                .HasColumnType("decimal(18,2)");
        }
    }
}
