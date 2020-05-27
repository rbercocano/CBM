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
            builder.Property(p => p.PricePerGram)
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();
            builder.Property(p => p.CorpClientId)
                .HasColumnType("INT")
                .IsRequired();
        }
    }
}
