using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgrammersBlog.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedingCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "INSERT INTO Categories(Name, Description, Note, CreatedDate, CreatedByName, ModifiedDate, ModifiedByName, IsActive, IsDeleted) VALUES ('Python', 'The Most Up-to-Date Information About Python Language', 'Python Category', GETDATE(), 'Migration', GETDATE(), 'Migration', 1, 0)");

            migrationBuilder.Sql(
                "INSERT INTO Categories (Name, Description, Note, CreatedDate, CreatedByName, ModifiedDate, ModifiedByName, IsActive, IsDeleted) VALUES ('Java', 'The Most Up-to-Date Information About Java Language', 'Java Category', GETDATE(), 'Migration', GETDATE(), 'Migration', 1, 0)");

            migrationBuilder.Sql(
                "INSERT INTO Categories (Name, Description, Note, CreatedDate, CreatedByName, ModifiedDate, ModifiedByName, IsActive, IsDeleted) VALUES ('Dart', 'The Most Up-to-Date Information About Dart Language', 'Dart Category', GETDATE(), 'Migration', GETDATE(), 'Migration', 1, 0)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
        }
    }
}
