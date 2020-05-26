using Charcutarie.Models.Entities;
using Charcutarie.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class OrderItemStatusConfiguration : IEntityTypeConfiguration<OrderItemStatus>
    {
        public void Configure(EntityTypeBuilder<OrderItemStatus> builder)
        {
            builder.ToTable("OrderItemStatus");
            builder.HasKey(p => p.OrderItemStatusId);

            builder.Property(p => p.OrderItemStatusId)
                .HasColumnType("INT")
                .IsRequired()
                 .HasConversion(v => (int)v,
                                v => (OrderItemStatusEnum)v);

            builder.Property(p => p.Description)
                .HasColumnType("VARCHAR(30)")
                .IsRequired();


        }
    }
}
