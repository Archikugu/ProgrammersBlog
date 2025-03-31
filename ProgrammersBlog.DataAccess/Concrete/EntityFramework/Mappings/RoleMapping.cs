using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.DataAccess.Concrete.EntityFramework.Mappings;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();
        builder.Property(r => r.Name).HasMaxLength(30).IsRequired();
        builder.Property(r => r.Description).HasMaxLength(250).IsRequired();

        builder.Property(r => r.CreatedByName).HasMaxLength(50).IsRequired();
        builder.Property(r => r.ModifiedByName).HasMaxLength(50).IsRequired();
        builder.Property(r => r.CreatedDate).HasDefaultValueSql("GETDATE()").IsRequired();
        builder.Property(r => r.IsActive).IsRequired();
        builder.Property(r => r.IsDeleted).IsRequired();
        builder.Property(r => r.Note).HasMaxLength(500);

        builder.ToTable("Roles");

        builder.HasData(new Role
        {
            Id = 1,
            Name = "Admin",
            Description = "Admin Role",
            IsActive = true,
            IsDeleted = false,
            CreatedByName = "InitialCreate",
            CreatedDate = new DateTime(2025, 3, 31),
            ModifiedByName = "InitialCreate",
            ModifiedDate = new DateTime(2025, 3, 31),
            Note = "Admin Role"
        });
    }
}
