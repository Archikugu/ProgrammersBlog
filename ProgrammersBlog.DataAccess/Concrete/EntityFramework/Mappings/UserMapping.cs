using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Entities.Concrete;
using System.Text;

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

        builder.HasData(new User
        {
            Id = 1,
            RoleId = 1,
            FirstName = "Gökhan",
            LastName = "Gök",
            UserName = "engokhangok",
            Email = "engokhangok@gmail.com",
            Note = "First Admin User",
            IsActive = true,
            IsDeleted = false,
            CreatedByName = "InitialCreate",
            CreatedDate = new DateTime(2025, 3, 31),
            ModifiedByName = "InitialCreate",
            ModifiedDate = new DateTime(2025, 3, 31),
            Description = "First Admin User",
            PasswordHash = Encoding.ASCII.GetBytes("3b612c75a7b5048a435fb6ec81e52ff92d6d795a8b5a9c17070f6a63c97a53b2"),
            Picture = "https://img.freepik.com/free-vector/man-having-online-job-interview_52683-43379.jpg?t=st=1743444144~exp=1743447744~hmac=08629ce7c5a7d6ae18d51ac2dc73c0dc75c9c63029b7b90dd6c000c865c31c1d&w=740"
        });
    }
}
