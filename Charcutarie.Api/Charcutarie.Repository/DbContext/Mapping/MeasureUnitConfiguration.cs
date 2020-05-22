using Charcutarie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class MeasureUnitConfiguration : IEntityTypeConfiguration<MeasureUnit>
    {
        public void Configure(EntityTypeBuilder<MeasureUnit> builder)
        {
            builder.ToTable("MeasureUnit");
            builder.HasKey(p => p.MeasureUnitId);

            builder.Property(p => p.Description)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(p => p.ShortName)
                .HasColumnType("VARCHAR(10)")
                .IsRequired();

        }
    }
}
