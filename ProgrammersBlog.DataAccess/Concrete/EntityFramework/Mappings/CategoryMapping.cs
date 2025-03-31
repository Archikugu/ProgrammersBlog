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
        }
    }
}
