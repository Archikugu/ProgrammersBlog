using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgrammersBlog.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedingArticles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
     "INSERT INTO Articles (Title,[Content],Note,Thumbnail,[Date],[CreatedDate],CreatedByName,ModifiedDate,ModifiedByName,IsActive,IsDeleted,UserId,CategoryId,SeoAuthor,SeoDescription,SeoTags,CommentCount,ViewsCount) VALUES " +
     "('What''s New in .NET 9?','With the release of .NET 9, developers now have access to improved cloud-native support, performance enhancements, and simplified development workflows. Native AOT improvements, new diagnostics, and updated APIs are some highlights.','.NET 9 Innovations','postImages/defaultThumbnail.jpg',GETDATE(),GETDATE(),'Migration',GETDATE(),'Migration',1,0,1,(SELECT TOP 1 Id FROM Categories ORDER BY Id ASC),'Microsoft Dev Team','Explore the latest features and performance updates in .NET 9','dotnet 9, .NET 9, Microsoft, AOT, cloud-native',0,150)");

            migrationBuilder.Sql(
                "INSERT INTO Articles (Title,[Content],Note,Thumbnail,[Date],[CreatedDate],CreatedByName,ModifiedDate,ModifiedByName,IsActive,IsDeleted,UserId,CategoryId,SeoAuthor,SeoDescription,SeoTags,CommentCount,ViewsCount) VALUES " +
                "('Getting Started with Blazor in .NET 9','Blazor now supports even faster rendering and WebAssembly improvements in .NET 9. Learn how to create interactive SPA applications without JavaScript.','Blazor Basics','postImages/defaultThumbnail.jpg',GETDATE(),GETDATE(),'Migration',GETDATE(),'Migration',1,0,1,(SELECT TOP 1 Id FROM Categories ORDER BY Id ASC),'Web Dev Team','Build fast, modern web apps using Blazor and .NET 9','Blazor, WebAssembly, .NET 9, SPA, C# Web Development',0,120)");

            migrationBuilder.Sql(
                "INSERT INTO Articles (Title,[Content],Note,Thumbnail,[Date],[CreatedDate],CreatedByName,ModifiedDate,ModifiedByName,IsActive,IsDeleted,UserId,CategoryId,SeoAuthor,SeoDescription,SeoTags,CommentCount,ViewsCount) VALUES " +
                "('Entity Framework Core 9 Performance Tips','EF Core 9 introduces major performance enhancements including compiled queries and JSON column mapping. Learn how to fine-tune your data access layer for scalable apps.','EF Core 9 Improvements','postImages/defaultThumbnail.jpg',GETDATE(),GETDATE(),'Migration',GETDATE(),'Migration',1,0,1,(SELECT TOP 1 Id FROM Categories ORDER BY Id ASC),'Data Team','Enhance your app''s performance with EF Core 9 best practices','Entity Framework Core 9, EF Core, .NET 9, ORM Performance',0,98)");

            migrationBuilder.Sql(
                "INSERT INTO Articles (Title,[Content],Note,Thumbnail,[Date],[CreatedDate],CreatedByName,ModifiedDate,ModifiedByName,IsActive,IsDeleted,UserId,CategoryId,SeoAuthor,SeoDescription,SeoTags,CommentCount,ViewsCount) VALUES " +
                "('C# 13 Features You Should Know','C# 13 brings pattern matching enhancements, primary constructors for all types, and improved lambda support. Explore the most useful features for cleaner and faster code.','C# 13 Highlights','postImages/defaultThumbnail.jpg',GETDATE(),GETDATE(),'Migration',GETDATE(),'Migration',1,0,1,(SELECT TOP 1 Id FROM Categories ORDER BY Id ASC),'Roslyn Team','Learn what''s new in C# 13 and how to use it effectively','C# 13, .NET 9, Pattern Matching, Primary Constructors',0,110)");

            migrationBuilder.Sql(
                "INSERT INTO Articles (Title,[Content],Note,Thumbnail,[Date],[CreatedDate],CreatedByName,ModifiedDate,ModifiedByName,IsActive,IsDeleted,UserId,CategoryId,SeoAuthor,SeoDescription,SeoTags,CommentCount,ViewsCount) VALUES " +
                "('Building Cross-Platform Apps with .NET MAUI','.NET MAUI allows developers to build native mobile and desktop apps from a single codebase. This article shows how to set up and deploy your first MAUI app in 2025.','.NET MAUI Guide','postImages/defaultThumbnail.jpg',GETDATE(),GETDATE(),'Migration',GETDATE(),'Migration',1,0,1,(SELECT TOP 1 Id FROM Categories ORDER BY Id ASC),'Xamarin Team','Develop cross-platform apps with .NET MAUI in .NET 9','.NET MAUI, Xamarin, Mobile Development, Cross-platform',0,105)");

            migrationBuilder.Sql(
                "INSERT INTO Articles (Title,[Content],Note,Thumbnail,[Date],[CreatedDate],CreatedByName,ModifiedDate,ModifiedByName,IsActive,IsDeleted,UserId,CategoryId,SeoAuthor,SeoDescription,SeoTags,CommentCount,ViewsCount) VALUES " +
                "('Integrating OpenAI with ASP.NET Core','Artificial intelligence is becoming essential in web development. Learn how to integrate OpenAI''s API into your ASP.NET Core apps for chat, summarization, and more.','AI in ASP.NET Core','postImages/defaultThumbnail.jpg',GETDATE(),GETDATE(),'Migration',GETDATE(),'Migration',1,0,1,(SELECT TOP 1 Id FROM Categories ORDER BY Id ASC),'AI Integration Team','Use OpenAI services within ASP.NET Core apps effectively','OpenAI, ChatGPT API, ASP.NET Core, AI Integration, .NET 9',0,140)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
