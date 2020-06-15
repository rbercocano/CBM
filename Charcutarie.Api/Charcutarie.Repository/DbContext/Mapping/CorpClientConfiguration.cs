using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class CorpClientConfiguration : IEntityTypeConfiguration<CorpClient>
    {
        public void Configure(EntityTypeBuilder<CorpClient> builder)
        {
            builder.ToTable("CorpClient");
            builder.HasKey(p => p.CorpClientId);
            builder.Property(p => p.CorpClientId).UseIdentityColumn();

            builder.Property(p => p.Name)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();
            builder.Property(p => p.Currency)
                .HasColumnType("VARCHAR(4)")
                .IsRequired();

            builder.Property(p => p.DBAName)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(p => p.CreatedOn)
                .HasColumnType("DATETIME")
                .IsRequired();

            builder.Property(p => p.Active)
                .HasColumnType("BIT")
                .IsRequired();

        }
    }
}
