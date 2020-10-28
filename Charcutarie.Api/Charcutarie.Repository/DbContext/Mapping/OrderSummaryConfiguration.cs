using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class OrderSummaryConfiguration : IEntityTypeConfiguration<OrderSummary>
    {
        public void Configure(EntityTypeBuilder<OrderSummary> builder)
        {
            builder.HasKey(p => p.OrderId);

            builder.Property(p => p.OrderNumber)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.CustomerTypeId)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.CustomerId)
                .HasColumnType("BIGINT")
                .IsRequired();
            builder.Property(p => p.Name)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
            builder.Property(p => p.CreatedOn)
                .HasColumnType("DateTimeOffset")
                .IsRequired();
            builder.Property(p => p.PaidOn)
                .HasColumnType("DateTimeOffset");
            builder.Property(p => p.CompleteBy)
                .HasColumnType("DATETIME")
                .IsRequired();
            builder.Property(p => p.OrderStatus)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
            builder.Property(p => p.PaymentStatus)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();
            builder.Property(p => p.SocialIdentifier)
                .HasColumnType("VARCHAR(MAX)");
            builder.Property(p => p.FinalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
