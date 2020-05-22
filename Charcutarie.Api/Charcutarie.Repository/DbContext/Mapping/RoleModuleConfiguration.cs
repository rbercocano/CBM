using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class RoleModuleConfiguration : IEntityTypeConfiguration<RoleModule>
    {
        public void Configure(EntityTypeBuilder<RoleModule> builder)
        {
            builder.ToTable("RoleModule");
            builder.HasKey(p => p.RoleModuleId);

            builder.Property(p => p.RoleId)
                .HasColumnType("INT");
            builder.Property(p => p.SystemModuleId)
                .HasColumnType("INT");

            builder.HasOne(p => p.Role)
                .WithMany(p => p.RoleModules);
            builder.HasOne(p => p.SystemModule)
                .WithMany(p => p.RoleModules);
        }
    }
}
