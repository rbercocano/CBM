using Charcutarie.Core.Security;
using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(p => p.UserId);

            builder.Property(p => p.Username)
                .HasColumnType("VARCHAR(20)")
                .IsRequired();
            builder.Property(p => p.Name)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();
            builder.Property(p => p.Active)
                .HasColumnType("BIT")
                .IsRequired();
            builder.Property(p => p.RoleId)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.CorpClientId)
                .HasColumnType("INT");
            builder.Property(p => p.Password)
                .HasColumnType("VARCHAR(50)")
                .IsRequired()
                .HasConversion(v => Password.Encrypt(v), v => Password.Decrypt(v));
            builder.Property(p => p.CreatedOn)
                .HasColumnType("DATETIME")
                .IsRequired();
            builder.Property(p => p.LastUpdated)
                .HasColumnType("DATETIME");


        }
    }
}
