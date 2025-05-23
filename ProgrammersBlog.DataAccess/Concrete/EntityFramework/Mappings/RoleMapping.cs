﻿using Microsoft.EntityFrameworkCore;
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
        builder.ToTable("AspNetRoles");

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
            new Role
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "4b82c981-4a0b-4d84-9f5e-cdc69c0ad2d1"
                //ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new Role
            {
                Id = 2,
                Name = "Editor",
                NormalizedName = "EDITOR",
                ConcurrencyStamp = "9c7c3c6d-9137-4f0d-8e0d-47fbe677b0f8"
                //ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        );
    }
}
