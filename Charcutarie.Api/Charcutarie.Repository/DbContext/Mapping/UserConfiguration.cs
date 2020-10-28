using Charcutarie.Core.Security;
using Charcutarie.Models.Entities;
using Charcutarie.Models.Enums;
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
                .HasColumnType("DateTimeOffset")
                .IsRequired();
            builder.Property(p => p.LastUpdated)
                .HasColumnType("DateTimeOffset");

            builder.Property(p => p.DateOfBirth)
                .HasColumnType("DATETIME");
            builder.Property(p => p.LastName)
                .HasColumnType("VARCHAR(100)");
            builder.Property(p => p.HomePhone)
                .HasColumnType("VARCHAR(20)");
            builder.Property(p => p.Mobile)
                .HasColumnType("VARCHAR(20)");

            builder.Property(p => p.DefaultVolumeMid)
                .IsRequired()
                 .HasConversion(v => (int)v,
                                v => (MeasureUnitEnum)v);

            builder.Property(p => p.DefaultMassMid)
                .IsRequired()
                 .HasConversion(v => (int)v,
                                v => (MeasureUnitEnum)v);

            builder.HasOne(p => p.MassMeasureUnit)
                .WithMany()
                .HasForeignKey(p => p.DefaultMassMid);

            builder.HasOne(p => p.VolumeMeasureUnit)
                .WithMany()
                .HasForeignKey(p => p.DefaultVolumeMid);
        }
    }
}
