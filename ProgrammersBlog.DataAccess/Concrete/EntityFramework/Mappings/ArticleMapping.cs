using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.DataAccess.Concrete.EntityFramework.Mappings
{
    public class ArticleMapping : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id).ValueGeneratedOnAdd(); 

            builder.Property(a => a.Title).HasMaxLength(100).IsRequired();

            builder.Property(a => a.Content).HasColumnType("NVARCHAR(MAX)").IsRequired();

            builder.Property(a => a.Date).IsRequired();

            builder.Property(a => a.SeoAuthor).HasMaxLength(50).IsRequired();

            builder.Property(a => a.SeoDescription).HasMaxLength(150).IsRequired();

            builder.Property(a => a.SeoTags).HasMaxLength(70).IsRequired();

            builder.Property(a => a.ViewsCount).IsRequired();

            builder.Property(a => a.CommentCount).IsRequired();

            builder.Property(a => a.Thumbnail).HasMaxLength(250).IsRequired();

            builder.Property(a => a.CreatedByName).HasMaxLength(50).IsRequired();
            builder.Property(a => a.ModifiedByName).HasMaxLength(50).IsRequired();
            builder.Property(a => a.CreatedDate).HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(a => a.ModifiedDate).IsRequired();
            builder.Property(a => a.IsActive).IsRequired();
            builder.Property(a => a.IsDeleted).IsRequired();
            builder.Property(a => a.Note).HasMaxLength(500);

            builder.HasOne<Category>(a => a.Category)
                   .WithMany(c => c.Articles)
                   .HasForeignKey(a => a.CategoryId);

            builder.HasOne<User>(a => a.User)
                   .WithMany(u => u.Articles)
                   .HasForeignKey(a => a.UserId);

            builder.ToTable("Articles");
        }
    }
}
