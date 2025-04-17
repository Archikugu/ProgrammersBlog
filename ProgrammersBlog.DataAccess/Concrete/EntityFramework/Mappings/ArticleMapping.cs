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

            //builder.HasData(
            //    new Article
            //    {
            //        Id = 1,
            //        CategoryId = 1,
            //        UserId = 1,
            //        Title = "C# 12 and .NET 8 - New Features and Improvements",
            //        Content = "C# 12 and .NET 8 bring significant improvements and new features. In this article, we will explore primary constructors, collection expressions, and performance enhancements in .NET 8.",
            //        Thumbnail = "Default.jpg",
            //        SeoDescription = "Explore the latest features of C# 12 and .NET 8, including primary constructors, collection expressions, and performance optimizations.",
            //        SeoTags = "C#, C# 12, .NET 8, .NET Core, Modern C#",
            //        SeoAuthor = "engokhangok",
            //        Date = new DateTime(2025, 3, 31),
            //        IsActive = true,
            //        IsDeleted = false,
            //        CreatedByName = "InitialCreate",
            //        CreatedDate = new DateTime(2025, 3, 31),
            //        ModifiedByName = "InitialCreate",
            //        ModifiedDate = new DateTime(2025, 3, 31),
            //        Note = "C# Category",
            //        ViewsCount=100,
            //        CommentCount = 1
            //    },

            //    new Article
            //    {
            //        Id = 2,
            //        CategoryId = 2,
            //        UserId = 1,
            //        Title = "C++ 20 and Modern C++ - New Features and Improvements",
            //        Content = "C++ 20 introduces many new features. In this article, we will examine these improvements, including concepts, ranges, coroutines, and modules.",
            //        Thumbnail = "Default.jpg",
            //        SeoDescription = "C++ 20 introduces new features like concepts, ranges, coroutines, and modules. This article explores these improvements.",
            //        SeoTags = "C++, C++ 20, Modern C++, Programming",
            //        SeoAuthor = "engokhangok",
            //        Date = new DateTime(2025, 3, 31),
            //        IsActive = true,
            //        IsDeleted = false,
            //        CreatedByName = "InitialCreate",
            //        CreatedDate = new DateTime(2025, 3, 31),
            //        ModifiedByName = "InitialCreate",
            //        ModifiedDate = new DateTime(2025, 3, 31),
            //        Note = "C++ Category",
            //        ViewsCount = 250,
            //        CommentCount = 1
            //    },
            //    new Article
            //    {
            //        Id = 3,
            //        CategoryId = 3,
            //        UserId = 1,
            //        Title = "JavaScript ES6+ - New Features and Improvements",
            //        Content = "JavaScript ES6 and later versions introduced many new features. In this article, we will explore arrow functions, async/await, destructuring, modules, and more.",
            //        Thumbnail = "Default.jpg",
            //        SeoDescription = "Learn about modern JavaScript features like ES6 arrow functions, async/await, modules, and more in this article.",
            //        SeoTags = "JavaScript, ES6, Modern JS, Async, Modules",
            //        SeoAuthor = "engokhangok",
            //        Date = new DateTime(2025, 3, 31),
            //        IsActive = true,
            //        IsDeleted = false,
            //        CreatedByName = "InitialCreate",
            //        CreatedDate = new DateTime(2025, 3, 31),
            //        ModifiedByName = "InitialCreate",
            //        ModifiedDate = new DateTime(2025, 3, 31),
            //        Note = "JavaScript Category",
            //        ViewsCount = 120,
            //        CommentCount = 1
            //    }
            //);
        }
    }
}
