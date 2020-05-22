using Charcutarie.Core.Security;
using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("UserToken");
            builder.HasKey(p => p.UserId);

            builder.Property(p => p.Token)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();

            builder.Property(p => p.CreatedOn)
                .HasColumnType("DATETIME")
                .IsRequired();

        }
    }
}
