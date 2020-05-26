using Charcutarie.Models.Entities;
using Charcutarie.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class PaymentStatusConfiguration : IEntityTypeConfiguration<PaymentStatus>
    {
        public void Configure(EntityTypeBuilder<PaymentStatus> builder)
        {
            builder.ToTable("PaymentStatus");
            builder.HasKey(p => p.PaymentStatusId);

            builder.Property(p => p.PaymentStatusId)
                .HasColumnType("INT")
                .IsRequired()
                 .HasConversion(v => (int)v,
                                v => (PaymentStatusEnum)v);
            builder.Property(p => p.Description)
                .HasColumnType("VARCHAR(30)")
                .IsRequired();


        }
    }
}
