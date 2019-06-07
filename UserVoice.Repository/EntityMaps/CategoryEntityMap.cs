using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UserVoice.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserVoice.Repository.EntityMaps
{
    public class CategoryEntityMap : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("category");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("Id").IsRequired();
            builder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(30);
            builder.Property(t => t.Remark).HasColumnName("Remark").HasMaxLength(500);
        }
    }
}
