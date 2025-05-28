using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.DataAccess.Concrete.EntityFramework.Mappings
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Name).HasMaxLength(70).IsRequired();

            builder.Property(c => c.Description).HasColumnType("NVARCHAR(MAX)").IsRequired();

            builder.Property(c => c.CreatedByName).HasMaxLength(50).IsRequired();
            builder.Property(c => c.ModifiedByName).HasMaxLength(50).IsRequired();
            builder.Property(c => c.CreatedDate).HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(c => c.ModifiedDate).IsRequired();
            builder.Property(c => c.IsActive).IsRequired();
            builder.Property(c => c.IsDeleted).IsRequired();
            builder.Property(c => c.Note).HasMaxLength(500);

            builder.ToTable("Categories");

            builder.HasData(
                 new Category
                 {
                     Id = 1,
                     Name = "C#",
                     Description = "C# dili ile .NET platformunda modern uygulamalar geliştirin.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2023, 10, 1),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2023, 10, 1),
                     Note = ".NET ile C# rehberi"
                 },
                 new Category
                 {
                     Id = 2,
                     Name = "C++",
                     Description = "Yüksek performanslı uygulamalar ve sistem programlama için C++.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2023, 10, 5),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2023, 10, 5),
                     Note = "C++ modern teknikleri"
                 },
                 new Category
                 {
                     Id = 3,
                     Name = "JavaScript",
                     Description = "Web geliştirme için en popüler betik dili JavaScript'i keşfedin.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2023, 10, 10),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2023, 10, 10),
                     Note = "Frontend JavaScript içerikleri"
                 },
                 new Category
                 {
                     Id = 4,
                     Name = "TypeScript",
                     Description = "JavaScript'e tip güvenliği kazandıran TypeScript ile güçlü uygulamalar yazın.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2023, 10, 15),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2023, 10, 15),
                     Note = "TypeScript eğitim serisi"
                 },
                 new Category
                 {
                     Id = 5,
                     Name = "Java",
                     Description = "Kurumsal uygulamalar geliştirmek için Java platformu ve Spring ekosistemi.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2023, 10, 20),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2023, 10, 20),
                     Note = "Java Spring ile geliştirme"
                 },
                 new Category
                 {
                     Id = 6,
                     Name = "Python",
                     Description = "Veri bilimi, yapay zeka ve web geliştirme için güçlü bir dil olan Python.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2023, 11, 1),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2023, 11, 1),
                     Note = "Python ile uygulamalı projeler"
                 },
                 new Category
                 {
                     Id = 7,
                     Name = "PHP",
                     Description = "Dinamik web sayfaları için popüler bir sunucu tarafı dili olan PHP.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2023, 11, 5),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2023, 11, 5),
                     Note = "PHP & Laravel geliştirme"
                 },
                 new Category
                 {
                     Id = 8,
                     Name = "Kotlin",
                     Description = "Android uygulamaları geliştirmek için modern ve güvenli bir dil.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2023, 11, 10),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2023, 11, 10),
                     Note = "Android Kotlin dersleri"
                 },
                 new Category
                 {
                     Id = 9,
                     Name = "Swift",
                     Description = "iOS ve macOS uygulamaları için Apple’ın modern programlama dili Swift.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2023, 11, 15),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2023, 11, 15),
                     Note = "Swift ile mobil geliştirme"
                 },
                 new Category
                 {
                     Id = 10,
                     Name = "Ruby",
                     Description = "Web uygulamaları geliştirmek için yalın ve güçlü bir dil: Ruby ve Rails.",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = new DateTime(2023, 11, 20),
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = new DateTime(2023, 11, 20),
                     Note = "Ruby on Rails eğitimi"
                 }
            );
        }
    }
}
