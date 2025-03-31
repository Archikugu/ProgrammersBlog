using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.DataAccess.Concrete.EntityFramework.Mappings
{
    public class CommentMapping : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Text).HasMaxLength(1000).IsRequired();

            builder.HasOne<Article>(c => c.Article)
                   .WithMany(a => a.Comments)
                   .HasForeignKey(c => c.ArticleId);

            builder.Property(c => c.CreatedByName).HasMaxLength(50).IsRequired();
            builder.Property(c => c.ModifiedByName).HasMaxLength(50).IsRequired();
            builder.Property(c => c.CreatedDate).HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(c => c.ModifiedDate).IsRequired();
            builder.Property(c => c.IsActive).IsRequired();
            builder.Property(c => c.IsDeleted).IsRequired();
            builder.Property(c => c.Note).HasMaxLength(500);

            builder.ToTable("Comments");

            builder.HasData(
                new Comment
                {
                    Id = 1,
                    ArticleId = 1,
                    Text = "Great explanation of C# 12 features!",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = new DateTime(2025, 3, 31),
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = new DateTime(2025, 3, 31),
                    Note = "C# Comment"
                },
                new Comment
                {
                    Id = 2,
                    ArticleId = 2,
                    Text = "C++ 20 concepts and coroutines are now much clearer to me!",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = new DateTime(2025, 3, 31),
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = new DateTime(2025, 3, 31),
                    Note = "C++ Comment"
                },
                new Comment
                {
                    Id = 3,
                    ArticleId = 3,
                    Text = "JavaScript ES6+ features like async/await and modules are well explained here!",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = new DateTime(2025, 3, 31),
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = new DateTime(2025, 3, 31),
                    Note = "JavaScript Comment"
                }
            );
        }
    }
}
