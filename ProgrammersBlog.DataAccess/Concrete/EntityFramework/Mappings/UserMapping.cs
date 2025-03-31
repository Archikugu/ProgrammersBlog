using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Entities.Concrete;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Property(u => u.FirstName).HasMaxLength(30).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(30).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(50).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.UserName).HasMaxLength(20).IsRequired();
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.Property(u => u.PasswordHash).HasColumnType("VARBINARY(500)").IsRequired();
        builder.Property(u => u.Description).HasMaxLength(500); 
        builder.Property(u => u.Picture).HasMaxLength(250).IsRequired();

        builder.HasOne<Role>(u => u.Role)
               .WithMany(r => r.Users)
               .HasForeignKey(u => u.RoleId);

        builder.Property(u => u.CreatedByName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.ModifiedByName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.CreatedDate).HasDefaultValueSql("GETDATE()").IsRequired();
        builder.Property(u => u.IsActive).IsRequired();
        builder.Property(u => u.IsDeleted).IsRequired();
        builder.Property(u => u.Note).HasMaxLength(500); 

        builder.ToTable("Users");
    }
}
