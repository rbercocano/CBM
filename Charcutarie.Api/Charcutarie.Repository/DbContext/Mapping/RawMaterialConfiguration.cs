using Charcutarie.Models.Entities;
using Charcutarie.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Charcutarie.Repository.DbContext.Mapping
{
    public class RawMaterialConfiguration : IEntityTypeConfiguration<RawMaterial>
    {
        public void Configure(EntityTypeBuilder<RawMaterial> builder)
        {
            builder.ToTable("RawMaterial");
            builder.HasKey(p => p.RawMaterialId);

            builder.Property(p => p.Name)
                .HasColumnType("VARCHAR(200)")
                .IsRequired();
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,4)")
                .IsRequired();
            builder.Property(p => p.CorpClientId)
                .HasColumnType("INT")
                .IsRequired();
            builder.Property(p => p.MeasureUnitId)
                .HasColumnType("INT")
                .IsRequired()
                 .HasConversion(v => (int)v,
                                v => (MeasureUnitEnum)v);
        }
    }
}
