﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Entities.Concrete;
using Microsoft.AspNetCore.Identity;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Picture).HasMaxLength(250).IsRequired();

        // Primary key
        builder.HasKey(u => u.Id);

        // Indexes for "normalized" username and email, to allow efficient lookups
        builder.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
        builder.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");

        // Maps to the AspNetUsers table
        builder.ToTable("AspNetUsers");

        // A concurrency token for use with the optimistic concurrency checking
        builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

        // Limit the size of columns to use efficient database types
        builder.Property(u => u.UserName).HasMaxLength(50);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(50);
        builder.Property(u => u.Email).HasMaxLength(256);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(50);

        // The relationships between User and other entity types
        // Note that these relationships are configured with no navigation properties

        // Each User can have many UserClaims
        builder.HasMany<UserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

        // Each User can have many UserLogins
        builder.HasMany<UserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

        // Each User can have many UserTokens
        builder.HasMany<UserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

        var adminUser = new User
        {
            Id = 1,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            PhoneNumber = "+901234567890",
            Picture = "default.png",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = "c871b9bb-8c2e-4f3e-8881-9f5f1635f8a9",
        };
        adminUser.PasswordHash = CreateHashPassword(adminUser, "Admin123*");

        var editorUser = new User
        {
            Id = 2,
            UserName = "editor",
            NormalizedUserName = "EDITOR",
            Email = "editor@gmail.com",
            NormalizedEmail = "EDITOR@GMAIL.COM",
            PhoneNumber = "+901234567890",
            Picture = "default.png",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = "5d8d2f57-0419-4aa6-b0f9-0fd94c6fc51b",
        };
        editorUser.PasswordHash = CreateHashPassword(editorUser, "Editor123*");

        builder.HasData(adminUser, editorUser);
    }
    private string CreateHashPassword(User user, string password)
    {
        var passwordHasher = new PasswordHasher<User>();
        return passwordHasher.HashPassword(user, password);
    }
}
