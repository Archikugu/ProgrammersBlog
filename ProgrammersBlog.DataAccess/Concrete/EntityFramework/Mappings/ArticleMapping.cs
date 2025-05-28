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

            builder.HasData(
                 new Article
                 {
                     Id = 1,
                     CategoryId = 1,
                     Title = "C# 9.0 ve .NET 5 Yenilikleri",
                     Content = "Bu makalede C# 9.0 ile gelen record yapısı, init accessor’lar ve pattern matching gibi yenilikler ele alınmıştır.",
                     Thumbnail = "postImages/defaultThumbnail.jpg",
                     SeoDescription = "C# 9.0 ve .NET 5 Yenilikleri",
                     SeoTags = "C#, C# 9, .NET5, .NET Framework, .NET Core",
                     SeoAuthor = "Alper Tunga",
                     Date = new DateTime(2025, 05, 01),
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2025, 05, 01),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2025, 05, 01),
                     Note = "C# 9.0 ve .NET 5 Yenilikleri",
                     UserId = 1,
                     ViewsCount = 100,
                     CommentCount = 0
                 },
                 new Article
                 {
                     Id = 2,
                     CategoryId = 2,
                     Title = "Python ile Veri Analizi",
                     Content = "Pandas ve NumPy kütüphaneleriyle veri analizi yapmayı öğrenin. Bu yazıda temel veri işleme adımlarını keşfedeceksiniz.",
                     Thumbnail = "postImages/defaultThumbnail.jpg",
                     SeoDescription = "Python ile Veri Analizi",
                     SeoTags = "Python, Veri Bilimi, Pandas, NumPy, Veri Analizi",
                     SeoAuthor = "Ayşe Yılmaz",
                     Date = new DateTime(2025, 05, 02),
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2025, 05, 02),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2025, 05, 02),
                     Note = "Python ile Veri Analizi",
                     UserId = 1,
                     ViewsCount = 150,
                     CommentCount = 0
                 },
                 new Article
                 {
                     Id = 3,
                     CategoryId = 3,
                     Title = "JavaScript ile Web Geliştirme",
                     Content = "Bu içerikte JavaScript diline giriş yapıyor, DOM manipülasyonu ve olay yönetimini örneklerle anlatıyoruz.",
                     Thumbnail = "postImages/defaultThumbnail.jpg",
                     SeoDescription = "JavaScript ile Web Geliştirme",
                     SeoTags = "JavaScript, Frontend, Web, DOM, Etkinlikler",
                     SeoAuthor = "Mehmet Can",
                     Date = new DateTime(2025, 05, 03),
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2025, 05, 03),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2025, 05, 03),
                     Note = "JavaScript ile Web Geliştirme",
                     UserId = 1,
                     ViewsCount = 200,
                     CommentCount = 0
                 },
                 new Article
                 {
                     Id = 4,
                     CategoryId = 4,
                     Title = "SQL ile Veri Tabanı Yönetimi",
                     Content = "SELECT, JOIN ve GROUP BY komutlarını kullanarak etkili SQL sorguları yazmayı bu yazıda öğreneceksiniz.",
                     Thumbnail = "postImages/defaultThumbnail.jpg",
                     SeoDescription = "SQL ile Veri Tabanı Yönetimi",
                     SeoTags = "SQL, Veri Tabanı, Sorgu, JOIN, SELECT",
                     SeoAuthor = "Zeynep Kaya",
                     Date = new DateTime(2025, 05, 04),
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2025, 05, 04),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2025, 05, 04),
                     Note = "SQL ile Veri Tabanı Yönetimi",
                     UserId = 1,
                     ViewsCount = 120,
                     CommentCount = 0
                 },
                 new Article
                 {
                     Id = 5,
                     CategoryId = 5,
                     Title = "HTML ve CSS ile Web Tasarımı",
                     Content = "HTML'in yapısal gücü ve CSS'in görsel düzenlemeleriyle modern web sayfaları tasarlayın.",
                     Thumbnail = "postImages/defaultThumbnail.jpg",
                     SeoDescription = "HTML ve CSS ile Web Tasarımı",
                     SeoTags = "HTML, CSS, Web Tasarımı, Frontend, UI",
                     SeoAuthor = "Ali Demir",
                     Date = new DateTime(2025, 05, 05),
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2025, 05, 05),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2025, 05, 05),
                     Note = "HTML ve CSS ile Web Tasarımı",
                     UserId = 1,
                     ViewsCount = 80,
                     CommentCount = 0
                 },
                 new Article
                 {
                     Id = 6,
                     CategoryId = 1,
                     Title = "ASP.NET Core ile Web API Geliştirme",
                     Content = "RESTful servislerin temelleri, Controller yapısı ve JSON veri formatıyla Web API oluşturma teknikleri anlatılmıştır.",
                     Thumbnail = "postImages/defaultThumbnail.jpg",
                     SeoDescription = "ASP.NET Core ile Web API Geliştirme",
                     SeoTags = "ASP.NET Core, Web API, REST, Backend",
                     SeoAuthor = "Cem Yıldız",
                     Date = new DateTime(2025, 05, 06),
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2025, 05, 06),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2025, 05, 06),
                     Note = "ASP.NET Core ile Web API Geliştirme",
                     UserId = 1,
                     ViewsCount = 170,
                     CommentCount = 0
                 },
                 new Article
                 {
                     Id = 7,
                     CategoryId = 2,
                     Title = "Makine Öğrenmesine Giriş",
                     Content = "Bu yazıda, makine öğrenmesinin temel prensipleri, algoritma türleri ve kullanım alanlarına genel bir bakış sunulmuştur.",
                     Thumbnail = "postImages/defaultThumbnail.jpg",
                     SeoDescription = "Makine Öğrenmesine Giriş",
                     SeoTags = "Machine Learning, Yapay Zeka, Python, Veri",
                     SeoAuthor = "Elif Kurt",
                     Date = new DateTime(2025, 05, 07),
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2025, 05, 07),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2025, 05, 07),
                     Note = "Makine Öğrenmesine Giriş",
                     UserId = 1,
                     ViewsCount = 190,
                     CommentCount = 0
                 },
                 new Article
                 {
                     Id = 8,
                     CategoryId = 3,
                     Title = "React ile SPA Uygulamaları",
                     Content = "React kütüphanesiyle tek sayfa uygulamaları (SPA) geliştirme adımları ve component yapısı detaylıca anlatılmıştır.",
                     Thumbnail = "postImages/defaultThumbnail.jpg",
                     SeoDescription = "React ile SPA Uygulamaları",
                     SeoTags = "React, SPA, Frontend, JavaScript, Web",
                     SeoAuthor = "Serkan Aydın",
                     Date = new DateTime(2025, 05, 08),
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2025, 05, 08),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2025, 05, 08),
                     Note = "React ile SPA Uygulamaları",
                     UserId = 1,
                     ViewsCount = 130,
                     CommentCount = 0
                 },
                 new Article
                 {
                     Id = 9,
                     CategoryId = 5,
                     Title = "Swift ile IOS Programlama",
                     Content = "iOS uygulamaları geliştirmek için Swift dilinin temelleri, Xcode kullanımı ve storyboard yapısı anlatılmıştır.",
                     Thumbnail = "postImages/defaultThumbnail.jpg",
                     SeoDescription = "Swift ile IOS Programlama",
                     SeoTags = "Swift, iOS, Apple, Mobil Geliştirme",
                     SeoAuthor = "Nazlı Tekin",
                     Date = new DateTime(2025, 05, 09),
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2025, 05, 09),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2025, 05, 09),
                     Note = "Swift ile IOS Programlama",
                     UserId = 1,
                     ViewsCount = 110,
                     CommentCount = 0
                 },
                 new Article
                 {
                     Id = 10,
                     CategoryId = 4,
                     Title = "Ruby ile Web Geliştirme",
                     Content = "Ruby dili ve Ruby on Rails framework’ü ile web uygulamaları geliştirme süreci adım adım anlatılmaktadır.",
                     Thumbnail = "postImages/defaultThumbnail.jpg",
                     SeoDescription = "Ruby ile Web Geliştirme",
                     SeoTags = "Ruby, Rails, Web Geliştirme, Backend",
                     SeoAuthor = "Berna Şahin",
                     Date = new DateTime(2025, 05, 10),
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2025, 05, 10),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2025, 05, 10),
                     Note = "Ruby ile Web Geliştirme",
                     UserId = 1,
                     ViewsCount = 95,
                     CommentCount = 0
                 }
            );
        }
    }
}
