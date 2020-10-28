using Charcutarie.Models.Entities;
using Charcutarie.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class BalanceConfiguration : IEntityTypeConfiguration<Balance>
    {
        public void Configure(EntityTypeBuilder<Balance> builder)
        {
            builder.HasKey(p => p.RID);
            builder.Property(p => p.RID)
                .HasColumnType("BIGINT");
            builder.Property(p => p.TransactionId)
                .HasColumnType("BIGINT");

            builder.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");
            builder.Property(p => p.RemainingBalance)
                .HasColumnType("decimal(18,2)");
            builder.Property(p => p.TransactionDate)
                .HasColumnType("DateTimeOffset");
            builder.Property(p => p.Description)
                .HasColumnType("VARCHAR(200)");
            builder.Property(p => p.MerchantName)
                .HasColumnType("VARCHAR(100)");
            builder.Property(p => p.TransactionTypeId)
                .HasColumnType("INT");
            builder.Property(p => p.OrderId)
                .HasColumnType("BIGINT");
            builder.Property(p => p.UserId)
                .HasColumnType("BIGINT");
            builder.Property(p => p.Username)
                .HasColumnType("VARCHAR(100)");
            builder.Property(p => p.UserFullName)
                .HasColumnType("VARCHAR(100)");
            builder.Property(p => p.TransactionType)
                .HasColumnType("Varchar(100)");

        }
    }
}
