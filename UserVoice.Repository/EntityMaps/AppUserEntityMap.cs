using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UserVoice.Entity;

namespace UserVoice.Repository.EntityMaps
{
    public class AppUserEntityMap : IEntityTypeConfiguration<AppUserEntity>
    {
        public void Configure(EntityTypeBuilder<AppUserEntity> builder)
        {
            builder.ToTable("AppUser");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Email).HasColumnName("Email").IsRequired().HasMaxLength(30);
            builder.Property(t => t.Password).HasColumnName("Password").IsRequired().HasMaxLength(50);
            builder.Property(t => t.LastLoginTime).HasColumnName("LastLoginTime");
            builder.Property(t => t.FullName).HasColumnName("FullName").HasMaxLength(30);

        }
    }
}
