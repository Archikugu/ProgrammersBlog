using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.DataAccess.Concrete.EntityFramework.Mappings;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Primary key
        builder.HasKey(r => r.Id);

        // Index for "normalized" role name to allow efficient lookups
        builder.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();

        // Maps to the AspNetRoles table
        builder.ToTable("Roles");

        // A concurrency token for use with the optimistic concurrency checking
        builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

        // Limit the size of columns to use efficient database types
        builder.Property(u => u.Name).HasMaxLength(100);
        builder.Property(u => u.NormalizedName).HasMaxLength(100);

        // The relationships between Role and other entity types
        // Note that these relationships are configured with no navigation properties

        // Each Role can have many entries in the UserRole join table
        builder.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

        // Each Role can have many associated RoleClaims
        builder.HasMany<RoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

        builder.HasData(
            new Role { Id = 1, Name = "Category.Create", NormalizedName = "CATEGORY.CREATE", ConcurrencyStamp = "b1f3d1ad-73ef-4d88-b2f9-7a4f8de6315a" },
            new Role { Id = 2, Name = "Category.Read", NormalizedName = "CATEGORY.READ", ConcurrencyStamp = "3e2829cd-2c84-42fa-8b7d-46fdc6b7852e" },
            new Role { Id = 3, Name = "Category.Update", NormalizedName = "CATEGORY.UPDATE", ConcurrencyStamp = "cb84d622-b68e-4652-9a06-9930ac5f2a90" },
            new Role { Id = 4, Name = "Category.Delete", NormalizedName = "CATEGORY.DELETE", ConcurrencyStamp = "87ae055e-0e40-4ae2-b16b-f9fd7a0f237b" },
            new Role { Id = 5, Name = "Article.Create", NormalizedName = "ARTICLE.CREATE", ConcurrencyStamp = "2ffcb143-7aa2-4a0a-90cb-6939fc2fefb9" },
            new Role { Id = 6, Name = "Article.Read", NormalizedName = "ARTICLE.READ", ConcurrencyStamp = "16417743-e07d-4f91-9a5d-0ef321248e4f" },
            new Role { Id = 7, Name = "Article.Update", NormalizedName = "ARTICLE.UPDATE", ConcurrencyStamp = "edc3bc89-7192-4a89-b3d3-7e8b11527a30" },
            new Role { Id = 8, Name = "Article.Delete", NormalizedName = "ARTICLE.DELETE", ConcurrencyStamp = "7a7b7de1-369e-4e92-9e94-b037a3c51b3f" },
            new Role { Id = 9, Name = "User.Create", NormalizedName = "USER.CREATE", ConcurrencyStamp = "065cb9c1-5a9c-45e7-bcbc-7d4f1bb79862" },
            new Role { Id = 10, Name = "User.Read", NormalizedName = "USER.READ", ConcurrencyStamp = "3b1c64ff-8c16-4ea4-91cb-0a612cb2609e" },
            new Role { Id = 11, Name = "User.Update", NormalizedName = "USER.UPDATE", ConcurrencyStamp = "3f58eb11-d0a1-4fd6-8ee2-36bc74008350" },
            new Role { Id = 12, Name = "User.Delete", NormalizedName = "USER.DELETE", ConcurrencyStamp = "c2824ad5-e35e-4b49-ae0c-f1b8e3078c40" },
            new Role { Id = 13, Name = "Role.Create", NormalizedName = "ROLE.CREATE", ConcurrencyStamp = "a8e3d6a9-9085-4122-a2ff-1cb7279c7db0" },
            new Role { Id = 14, Name = "Role.Read", NormalizedName = "ROLE.READ", ConcurrencyStamp = "ff925ae8-0bc5-4f29-8ab7-b39e215f9a8f" },
            new Role { Id = 15, Name = "Role.Update", NormalizedName = "ROLE.UPDATE", ConcurrencyStamp = "3dcce071-e735-426e-bfb1-dcfe2f546623" },
            new Role { Id = 16, Name = "Role.Delete", NormalizedName = "ROLE.DELETE", ConcurrencyStamp = "b9aa0d9a-bdef-4399-b57c-3d2793e4fc68" },
            new Role { Id = 17, Name = "Comment.Create", NormalizedName = "COMMENT.CREATE", ConcurrencyStamp = "0fa12664-0c6d-4ac5-8018-95e96c9c82d4" },
            new Role { Id = 18, Name = "Comment.Read", NormalizedName = "COMMENT.READ", ConcurrencyStamp = "80ae1c0f-0900-4d56-9a0b-92c761da49d1" },
            new Role { Id = 19, Name = "Comment.Update", NormalizedName = "COMMENT.UPDATE", ConcurrencyStamp = "b7f69e2b-3089-4ee0-99de-4db71d58660f" },
            new Role { Id = 20, Name = "Comment.Delete", NormalizedName = "COMMENT.DELETE", ConcurrencyStamp = "1d8ce7b1-d21e-4c2e-b4d7-693b39b80cb2" },
            new Role { Id = 21, Name = "AdminArea.Home.Read", NormalizedName = "ADMINAREA.HOME.READ", ConcurrencyStamp = "d4797352-c3d8-4f10-a301-0dfd474bb4c1" },
            new Role { Id = 22, Name = "SuperAdmin", NormalizedName = "SUPERADMIN", ConcurrencyStamp = "e2bb9f0a-bbd1-4da5-9f8f-b5f6cba9c3c3" }
        );
    }
}
