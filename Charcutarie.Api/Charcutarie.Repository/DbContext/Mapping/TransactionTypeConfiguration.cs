using Charcutarie.Models.Entities;
using Charcutarie.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionType>
    {
        public void Configure(EntityTypeBuilder<TransactionType> builder)
        {
            builder.ToTable("TransactionType");
            builder.HasKey(p => p.TransactionTypeId);

            builder.Property(p => p.Description)
                .HasColumnType("VARCHAR(50)")
                .IsRequired();

            builder.Property(p => p.Type)
                .HasColumnType("VARCHAR(1)")
                .IsRequired();
        }
    }
}
