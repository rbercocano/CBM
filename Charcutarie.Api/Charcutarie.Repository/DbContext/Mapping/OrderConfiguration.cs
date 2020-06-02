using Charcutarie.Models.Entities;
using Charcutarie.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(p => p.OrderId);

            builder.Property(p => p.OrderNumber)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.CustomerId)
                .HasColumnType("BIGINT")
                .IsRequired();
            builder.Property(p => p.CreatedOn)
                .HasColumnType("DATETIME")
                .IsRequired();
            builder.Property(p => p.LastUpdated)
                .HasColumnType("DATETIME");
            builder.Property(p => p.CompleteBy)
                .HasColumnType("DATE")
                .IsRequired();
            builder.Property(p => p.OrderStatusId)
                .HasColumnType("INT")
                .IsRequired()
                 .HasConversion(v => (int)v,
                                v => (OrderStatusEnum)v);
            builder.Property(p => p.PaymentStatusId)
                .HasColumnType("INT")
                .IsRequired()
                 .HasConversion(v => (int)v,
                                v => (PaymentStatusEnum)v);
            builder.Property(p => p.FreightPrice)
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();
            builder.Property(p => p.PaidOn)
                .HasColumnType("DATE");
            builder.HasMany(p => p.OrderItems)
                .WithOne(p => p.Order);
            builder.HasOne(p => p.OrderStatus)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.OrderStatusId);
            builder.HasOne(p => p.PaymentStatus)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.PaymentStatusId);

            builder.HasOne(p => p.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.CustomerId);
        }
    }
}
