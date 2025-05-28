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
                     Text = "C# dilinde yazılmış bu makale, temel sözdizimi ve OOP kavramları açısından oldukça bilgilendiriciydi. Yeni başlayanlar için harika bir kaynak olmuş.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2023, 11, 15),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2023, 11, 15),
                     Note = "C# Makale Yorumu"
                 },
                 new Comment
                 {
                     Id = 2,
                     ArticleId = 2,
                     Text = "C++ konusuna oldukça derinlemesine değinilmiş. Hafıza yönetimi ve pointerlar hakkında verdiğiniz örnekler çok açıklayıcıydı.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2023, 12, 2),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2023, 12, 2),
                     Note = "C++ Makale Yorumu"
                 },
                 new Comment
                 {
                     Id = 3,
                     ArticleId = 3,
                     Text = "JavaScript'in async/await yapısı ve event loop mekanizması çok iyi anlatılmış. Özellikle yeni başlayanlar için sade bir anlatım olmuş.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2024, 1, 10),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2024, 1, 10),
                     Note = "JavaScript Makale Yorumu"
                 },
                 new Comment
                 {
                     Id = 4,
                     ArticleId = 4,
                     Text = "TypeScript ile tip güvenliğini sağlamak büyük avantaj. Bu yazı sayesinde projeme kolayca TypeScript entegre edebildim.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2024, 2, 5),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2024, 2, 5),
                     Note = "TypeScript Makale Yorumu"
                 },
                 new Comment
                 {
                     Id = 5,
                     ArticleId = 5,
                     Text = "Java'nın çoklu platform desteği ve JVM hakkında verdiğiniz teknik bilgiler oldukça doyurucuydu. Emeğinize sağlık!",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2024, 2, 22),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2024, 2, 22),
                     Note = "Java Makale Yorumu"
                 },
                 new Comment
                 {
                     Id = 6,
                     ArticleId = 6,
                     Text = "Python’un yalın sözdizimi ve veri analizi konularındaki gücünü çok iyi özetlemişsiniz. Pandas örnekleri çok faydalıydı.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2024, 3, 3),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2024, 3, 3),
                     Note = "Python Makale Yorumu"
                 },
                 new Comment
                 {
                     Id = 7,
                     ArticleId = 7,
                     Text = "PHP ile backend geliştirme konusunda bu kadar net ve güncel bilgiler görmek sevindirici. Laravel konusuna da değinmeniz harika olmuş.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2024, 3, 20),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2024, 3, 20),
                     Note = "PHP Makale Yorumu"
                 },
                 new Comment
                 {
                     Id = 8,
                     ArticleId = 8,
                     Text = "Kotlin’in Android geliştirmede Java’ya göre sağladığı avantajları çok güzel anlatmışsınız. Özellikle null safety konusu çok iyi işlenmiş.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2024, 4, 4),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2024, 4, 4),
                     Note = "Kotlin Makale Yorumu"
                 },
                 new Comment
                 {
                     Id = 9,
                     ArticleId = 9,
                     Text = "Swift ile iOS uygulama geliştirme süreçleri hakkında verdiğiniz detaylar oldukça açıklayıcı. SwiftUI konusuna da değinmeniz çok iyi olmuş.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2024, 4, 18),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2024, 4, 18),
                     Note = "Swift Makale Yorumu"
                 },
                 new Comment
                 {
                     Id = 10,
                     ArticleId = 10,
                     Text = "Ruby’nin yazım kolaylığı ve Rails framework’ünün güçlü yapısı net bir şekilde aktarılmış. Özellikle RESTful API mimarisi anlatımı çok başarılı.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2024, 5, 1),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2024, 5, 1),
                     Note = "Ruby Makale Yorumu"
                 }
            );

        }
    }
}
