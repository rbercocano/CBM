using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class SystemModuleConfiguration : IEntityTypeConfiguration<SystemModule>
    {
        public void Configure(EntityTypeBuilder<SystemModule> builder)
        {
            builder.ToTable("SystemModule");
            builder.HasKey(p => p.SystemModuleId);

            builder.Property(p => p.ParentId)
                .HasColumnType("INT");
            builder.Property(p => p.Name)
                .HasColumnType("VARCHAR(50)")
                .IsRequired();
            builder.Property(p => p.Route)
                .HasColumnType("VARCHAR(100)");
            builder.Property(p => p.Active)
                .HasColumnType("BIT")
                .IsRequired();

            builder.HasOne(p => p.ParentModule)
                .WithMany(p => p.ChildModules)
                .HasForeignKey(p => p.ParentId);

            builder.HasMany(p => p.RoleModules)
                .WithOne(p => p.SystemModule);
            builder.Property(p => p.Order)
                .HasColumnType("INT")
                .IsRequired();
        }
    }
}
