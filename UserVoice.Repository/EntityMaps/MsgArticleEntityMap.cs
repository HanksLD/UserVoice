using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserVoice.Entity;

namespace UserVoice.Repository.EntityMaps
{
    public class MsgArticleEntityMap : IEntityTypeConfiguration<MsgArticleEntity>
    {
        public void Configure(EntityTypeBuilder<MsgArticleEntity> builder)
        {
            builder.ToTable("messagearticle");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Author).HasColumnName("Author").IsRequired().HasMaxLength(30);
            builder.Property(t => t.CategoryId).HasColumnName("CategoryId").IsRequired();
            builder.Property(t => t.CreatedTime).HasColumnName("CreatedTime").IsRequired();
            builder.Property(t => t.MsgContent).HasColumnName("MsgContent").IsRequired().HasMaxLength(500);
            builder.Property(t => t.Title).HasColumnName("Title").IsRequired().HasMaxLength(50);

            builder.HasOne<CategoryEntity>(t => t.Category)
                .WithMany(p => p.Articles).HasForeignKey(p => p.CategoryId).IsRequired();
        }
    }
}
