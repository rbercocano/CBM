using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(p => p.RoleId);

            builder.Property(p => p.Name)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.HasMany(p => p.RoleModules)
                .WithOne(p => p.Role);
        }
    }
}
