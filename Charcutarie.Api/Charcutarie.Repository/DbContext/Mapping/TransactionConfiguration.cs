using Charcutarie.Models.Entities;
using Charcutarie.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transaction");
            builder.HasKey(p => p.TransactionId);

            builder.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.Date)
                .HasColumnType("DateTime")
                .IsRequired();
            builder.Property(p => p.Description)
                .HasColumnType("VARCHAR(200)")
                .IsRequired();
            builder.Property(p => p.MerchantName)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();
            builder.Property(p => p.TransactionTypeId)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.OrderId)
                .HasColumnType("BIGINT");
            builder.Property(p => p.IsOrderPrincipalAmount)
                .HasColumnType("BIT");
            builder.Property(p => p.CorpClientId)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.TransactionStatusId)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.UserId)
                .HasColumnType("BIGINT")
                .IsRequired();

            builder.HasOne(p => p.TransactionType)
                .WithMany()
                .HasForeignKey(p => p.TransactionTypeId);
            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);
        }
    }
}
