﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.DataAccess.Concrete.EntityFramework.Mappings;

public class UserClaimMapping : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        // Primary key
        builder.HasKey(uc => uc.Id);

        // Maps to the AspNetUserClaims table
        builder.ToTable("UserClaims");
    }
}
