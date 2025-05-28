using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProgrammersBlog.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Picture = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    About = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    YoutubeLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TwitterLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InstagramLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FacebookLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LinkedInLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    GitHubLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    WebsiteLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    Thumbnail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ViewsCount = table.Column<int>(type: "int", nullable: false),
                    CommentCount = table.Column<int>(type: "int", nullable: false),
                    SeoAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SeoDescription = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SeoTags = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedByName", "CreatedDate", "Description", "IsActive", "IsDeleted", "ModifiedByName", "ModifiedDate", "Name", "Note" },
                values: new object[,]
                {
                    { 1, "InitialCreate", new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C# dili ile .NET platformunda modern uygulamalar geliştirin.", true, false, "InitialCreate", new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C#", ".NET ile C# rehberi" },
                    { 2, "InitialCreate", new DateTime(2023, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yüksek performanslı uygulamalar ve sistem programlama için C++.", true, false, "InitialCreate", new DateTime(2023, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "C++", "C++ modern teknikleri" },
                    { 3, "InitialCreate", new DateTime(2023, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Web geliştirme için en popüler betik dili JavaScript'i keşfedin.", true, false, "InitialCreate", new DateTime(2023, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "JavaScript", "Frontend JavaScript içerikleri" },
                    { 4, "InitialCreate", new DateTime(2023, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "JavaScript'e tip güvenliği kazandıran TypeScript ile güçlü uygulamalar yazın.", true, false, "InitialCreate", new DateTime(2023, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "TypeScript", "TypeScript eğitim serisi" },
                    { 5, "InitialCreate", new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kurumsal uygulamalar geliştirmek için Java platformu ve Spring ekosistemi.", true, false, "InitialCreate", new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Java", "Java Spring ile geliştirme" },
                    { 6, "InitialCreate", new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Veri bilimi, yapay zeka ve web geliştirme için güçlü bir dil olan Python.", true, false, "InitialCreate", new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Python", "Python ile uygulamalı projeler" },
                    { 7, "InitialCreate", new DateTime(2023, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dinamik web sayfaları için popüler bir sunucu tarafı dili olan PHP.", true, false, "InitialCreate", new DateTime(2023, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "PHP", "PHP & Laravel geliştirme" },
                    { 8, "InitialCreate", new DateTime(2023, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Android uygulamaları geliştirmek için modern ve güvenli bir dil.", true, false, "InitialCreate", new DateTime(2023, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kotlin", "Android Kotlin dersleri" },
                    { 9, "InitialCreate", new DateTime(2023, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "iOS ve macOS uygulamaları için Apple’ın modern programlama dili Swift.", true, false, "InitialCreate", new DateTime(2023, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Swift", "Swift ile mobil geliştirme" },
                    { 10, "InitialCreate", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Web uygulamaları geliştirmek için yalın ve güçlü bir dil: Ruby ve Rails.", true, false, "InitialCreate", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ruby", "Ruby on Rails eğitimi" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "b1f3d1ad-73ef-4d88-b2f9-7a4f8de6315a", "Category.Create", "CATEGORY.CREATE" },
                    { 2, "3e2829cd-2c84-42fa-8b7d-46fdc6b7852e", "Category.Read", "CATEGORY.READ" },
                    { 3, "cb84d622-b68e-4652-9a06-9930ac5f2a90", "Category.Update", "CATEGORY.UPDATE" },
                    { 4, "87ae055e-0e40-4ae2-b16b-f9fd7a0f237b", "Category.Delete", "CATEGORY.DELETE" },
                    { 5, "2ffcb143-7aa2-4a0a-90cb-6939fc2fefb9", "Article.Create", "ARTICLE.CREATE" },
                    { 6, "16417743-e07d-4f91-9a5d-0ef321248e4f", "Article.Read", "ARTICLE.READ" },
                    { 7, "edc3bc89-7192-4a89-b3d3-7e8b11527a30", "Article.Update", "ARTICLE.UPDATE" },
                    { 8, "7a7b7de1-369e-4e92-9e94-b037a3c51b3f", "Article.Delete", "ARTICLE.DELETE" },
                    { 9, "065cb9c1-5a9c-45e7-bcbc-7d4f1bb79862", "User.Create", "USER.CREATE" },
                    { 10, "3b1c64ff-8c16-4ea4-91cb-0a612cb2609e", "User.Read", "USER.READ" },
                    { 11, "3f58eb11-d0a1-4fd6-8ee2-36bc74008350", "User.Update", "USER.UPDATE" },
                    { 12, "c2824ad5-e35e-4b49-ae0c-f1b8e3078c40", "User.Delete", "USER.DELETE" },
                    { 13, "a8e3d6a9-9085-4122-a2ff-1cb7279c7db0", "Role.Create", "ROLE.CREATE" },
                    { 14, "ff925ae8-0bc5-4f29-8ab7-b39e215f9a8f", "Role.Read", "ROLE.READ" },
                    { 15, "3dcce071-e735-426e-bfb1-dcfe2f546623", "Role.Update", "ROLE.UPDATE" },
                    { 16, "b9aa0d9a-bdef-4399-b57c-3d2793e4fc68", "Role.Delete", "ROLE.DELETE" },
                    { 17, "0fa12664-0c6d-4ac5-8018-95e96c9c82d4", "Comment.Create", "COMMENT.CREATE" },
                    { 18, "80ae1c0f-0900-4d56-9a0b-92c761da49d1", "Comment.Read", "COMMENT.READ" },
                    { 19, "b7f69e2b-3089-4ee0-99de-4db71d58660f", "Comment.Update", "COMMENT.UPDATE" },
                    { 20, "1d8ce7b1-d21e-4c2e-b4d7-693b39b80cb2", "Comment.Delete", "COMMENT.DELETE" },
                    { 21, "d4797352-c3d8-4f10-a301-0dfd474bb4c1", "AdminArea.Home.Read", "ADMINAREA.HOME.READ" },
                    { 22, "e2bb9f0a-bbd1-4da5-9f8f-b5f6cba9c3c3", "SuperAdmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "About", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FacebookLink", "FirstName", "GitHubLink", "InstagramLink", "LastName", "LinkedInLink", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Picture", "SecurityStamp", "TwitterLink", "TwoFactorEnabled", "UserName", "WebsiteLink", "YoutubeLink" },
                values: new object[,]
                {
                    { 1, "Admin User of ProgrammersBlog", 0, "fb81de8d-4b39-40cc-9349-7b512cc77a3a", "admin@gmail.com", true, "https://facebook.com/adminuser", "Admin", "https://github.com/adminuser", "https://instagram.com/adminuser", "User", "https://linkedin.com/adminuser", false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEP+PO/V1Yipgyh1wCNG/jfCqRT9Bv0WRLm0rmA/6U3a5JxLQMUtJ0sZFlTjSX7Lpxg==", "+901234567890", true, "/userImages/defaultUser.png", "c871b9bb-8c2e-4f3e-8881-9f5f1635f8a9", "https://twitter.com/adminuser", false, "admin", "https://programmersblog.com/", "https://youtube.com/adminuser" },
                    { 2, "Editor User of ProgrammersBlog", 0, "e79c6d14-c475-403a-b2dd-7be21429fb7c", "editor@gmail.com", true, "https://facebook.com/editoruser", "Admin", "https://github.com/editoruser", "https://instagram.com/editoruser", "User", "https://linkedin.com/editoruser", false, null, "EDITOR@GMAIL.COM", "EDITOR", "AQAAAAIAAYagAAAAELZf2+9f846WWbSCl+vtGpk0YlZckybWEIHLWTqOFme5vJc1uAsu4r5QsNtEzBJSLg==", "+901234567890", true, "/userImages/defaultUser.png", "5d8d2f57-0419-4aa6-b0f9-0fd94c6fc51b", "https://twitter.com/editoruser", false, "editor", "https://programmersblog.com/", "https://youtube.com/editoruser" }
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "CommentCount", "Content", "CreatedByName", "CreatedDate", "Date", "IsActive", "IsDeleted", "ModifiedByName", "ModifiedDate", "Note", "SeoAuthor", "SeoDescription", "SeoTags", "Thumbnail", "Title", "UserId", "ViewsCount" },
                values: new object[,]
                {
                    { 1, 1, 0, "Bu makalede C# 9.0 ile gelen record yapısı, init accessor’lar ve pattern matching gibi yenilikler ele alınmıştır.", "InitialCreate", new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C# 9.0 ve .NET 5 Yenilikleri", "Alper Tunga", "C# 9.0 ve .NET 5 Yenilikleri", "C#, C# 9, .NET5, .NET Framework, .NET Core", "postImages/defaultThumbnail.jpg", "C# 9.0 ve .NET 5 Yenilikleri", 1, 100 },
                    { 2, 2, 0, "Pandas ve NumPy kütüphaneleriyle veri analizi yapmayı öğrenin. Bu yazıda temel veri işleme adımlarını keşfedeceksiniz.", "InitialCreate", new DateTime(2025, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2025, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Python ile Veri Analizi", "Ayşe Yılmaz", "Python ile Veri Analizi", "Python, Veri Bilimi, Pandas, NumPy, Veri Analizi", "postImages/defaultThumbnail.jpg", "Python ile Veri Analizi", 1, 150 },
                    { 3, 3, 0, "Bu içerikte JavaScript diline giriş yapıyor, DOM manipülasyonu ve olay yönetimini örneklerle anlatıyoruz.", "InitialCreate", new DateTime(2025, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2025, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "JavaScript ile Web Geliştirme", "Mehmet Can", "JavaScript ile Web Geliştirme", "JavaScript, Frontend, Web, DOM, Etkinlikler", "postImages/defaultThumbnail.jpg", "JavaScript ile Web Geliştirme", 1, 200 },
                    { 4, 4, 0, "SELECT, JOIN ve GROUP BY komutlarını kullanarak etkili SQL sorguları yazmayı bu yazıda öğreneceksiniz.", "InitialCreate", new DateTime(2025, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2025, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "SQL ile Veri Tabanı Yönetimi", "Zeynep Kaya", "SQL ile Veri Tabanı Yönetimi", "SQL, Veri Tabanı, Sorgu, JOIN, SELECT", "postImages/defaultThumbnail.jpg", "SQL ile Veri Tabanı Yönetimi", 1, 120 },
                    { 5, 5, 0, "HTML'in yapısal gücü ve CSS'in görsel düzenlemeleriyle modern web sayfaları tasarlayın.", "InitialCreate", new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "HTML ve CSS ile Web Tasarımı", "Ali Demir", "HTML ve CSS ile Web Tasarımı", "HTML, CSS, Web Tasarımı, Frontend, UI", "postImages/defaultThumbnail.jpg", "HTML ve CSS ile Web Tasarımı", 1, 80 },
                    { 6, 1, 0, "RESTful servislerin temelleri, Controller yapısı ve JSON veri formatıyla Web API oluşturma teknikleri anlatılmıştır.", "InitialCreate", new DateTime(2025, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2025, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core ile Web API Geliştirme", "Cem Yıldız", "ASP.NET Core ile Web API Geliştirme", "ASP.NET Core, Web API, REST, Backend", "postImages/defaultThumbnail.jpg", "ASP.NET Core ile Web API Geliştirme", 1, 170 },
                    { 7, 2, 0, "Bu yazıda, makine öğrenmesinin temel prensipleri, algoritma türleri ve kullanım alanlarına genel bir bakış sunulmuştur.", "InitialCreate", new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Makine Öğrenmesine Giriş", "Elif Kurt", "Makine Öğrenmesine Giriş", "Machine Learning, Yapay Zeka, Python, Veri", "postImages/defaultThumbnail.jpg", "Makine Öğrenmesine Giriş", 1, 190 },
                    { 8, 3, 0, "React kütüphanesiyle tek sayfa uygulamaları (SPA) geliştirme adımları ve component yapısı detaylıca anlatılmıştır.", "InitialCreate", new DateTime(2025, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2025, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "React ile SPA Uygulamaları", "Serkan Aydın", "React ile SPA Uygulamaları", "React, SPA, Frontend, JavaScript, Web", "postImages/defaultThumbnail.jpg", "React ile SPA Uygulamaları", 1, 130 },
                    { 9, 5, 0, "iOS uygulamaları geliştirmek için Swift dilinin temelleri, Xcode kullanımı ve storyboard yapısı anlatılmıştır.", "InitialCreate", new DateTime(2025, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2025, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Swift ile IOS Programlama", "Nazlı Tekin", "Swift ile IOS Programlama", "Swift, iOS, Apple, Mobil Geliştirme", "postImages/defaultThumbnail.jpg", "Swift ile IOS Programlama", 1, 110 },
                    { 10, 4, 0, "Ruby dili ve Ruby on Rails framework’ü ile web uygulamaları geliştirme süreci adım adım anlatılmaktadır.", "InitialCreate", new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ruby ile Web Geliştirme", "Berna Şahin", "Ruby ile Web Geliştirme", "Ruby, Rails, Web Geliştirme, Backend", "postImages/defaultThumbnail.jpg", "Ruby ile Web Geliştirme", 1, 95 }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 1 },
                    { 7, 1 },
                    { 8, 1 },
                    { 9, 1 },
                    { 10, 1 },
                    { 11, 1 },
                    { 12, 1 },
                    { 13, 1 },
                    { 14, 1 },
                    { 15, 1 },
                    { 16, 1 },
                    { 17, 1 },
                    { 18, 1 },
                    { 19, 1 },
                    { 20, 1 },
                    { 21, 1 },
                    { 22, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 2 },
                    { 5, 2 },
                    { 6, 2 },
                    { 7, 2 },
                    { 8, 2 },
                    { 17, 2 },
                    { 18, 2 },
                    { 19, 2 },
                    { 20, 2 },
                    { 21, 2 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "ArticleId", "CreatedByName", "CreatedDate", "IsActive", "IsDeleted", "ModifiedByName", "ModifiedDate", "Note", "Text" },
                values: new object[,]
                {
                    { 1, 1, "InitialCreate", new DateTime(2023, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2023, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "C# Makale Yorumu", "C# dilinde yazılmış bu makale, temel sözdizimi ve OOP kavramları açısından oldukça bilgilendiriciydi. Yeni başlayanlar için harika bir kaynak olmuş." },
                    { 2, 2, "InitialCreate", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "C++ Makale Yorumu", "C++ konusuna oldukça derinlemesine değinilmiş. Hafıza yönetimi ve pointerlar hakkında verdiğiniz örnekler çok açıklayıcıydı." },
                    { 3, 3, "InitialCreate", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "JavaScript Makale Yorumu", "JavaScript'in async/await yapısı ve event loop mekanizması çok iyi anlatılmış. Özellikle yeni başlayanlar için sade bir anlatım olmuş." },
                    { 4, 4, "InitialCreate", new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "TypeScript Makale Yorumu", "TypeScript ile tip güvenliğini sağlamak büyük avantaj. Bu yazı sayesinde projeme kolayca TypeScript entegre edebildim." },
                    { 5, 5, "InitialCreate", new DateTime(2024, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2024, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Java Makale Yorumu", "Java'nın çoklu platform desteği ve JVM hakkında verdiğiniz teknik bilgiler oldukça doyurucuydu. Emeğinize sağlık!" },
                    { 6, 6, "InitialCreate", new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Python Makale Yorumu", "Python’un yalın sözdizimi ve veri analizi konularındaki gücünü çok iyi özetlemişsiniz. Pandas örnekleri çok faydalıydı." },
                    { 7, 7, "InitialCreate", new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "PHP Makale Yorumu", "PHP ile backend geliştirme konusunda bu kadar net ve güncel bilgiler görmek sevindirici. Laravel konusuna da değinmeniz harika olmuş." },
                    { 8, 8, "InitialCreate", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kotlin Makale Yorumu", "Kotlin’in Android geliştirmede Java’ya göre sağladığı avantajları çok güzel anlatmışsınız. Özellikle null safety konusu çok iyi işlenmiş." },
                    { 9, 9, "InitialCreate", new DateTime(2024, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2024, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Swift Makale Yorumu", "Swift ile iOS uygulama geliştirme süreçleri hakkında verdiğiniz detaylar oldukça açıklayıcı. SwiftUI konusuna da değinmeniz çok iyi olmuş." },
                    { 10, 10, "InitialCreate", new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "InitialCreate", new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ruby Makale Yorumu", "Ruby’nin yazım kolaylığı ve Rails framework’ünün güçlü yapısı net bir şekilde aktarılmış. Özellikle RESTful API mimarisi anlatımı çok başarılı." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UserId",
                table: "Articles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
