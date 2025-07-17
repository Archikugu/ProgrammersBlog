using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgrammersBlog.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig_comment_note_nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Comments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Categories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Articles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "efb7d817-dfc2-41e4-bf5f-c26299c51a89", "AQAAAAIAAYagAAAAEM0OL08gQEOPiJUfPUfC+4gQFSEFKQjGqrNMsDJyoW4/HLLYNh8Es6VmeO3Ntz+YGw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ccb7bf89-ca3c-4d21-aea8-5f886599d518", "AQAAAAIAAYagAAAAEMe8kSl9nbrO6BW4zxbS2SfWcqLowOYS97yAxmy5z3aVh/hl1vf8bMs3UBP/teXISg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Comments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Categories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Articles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f08253e6-fbc3-4ed1-bfa3-abb2f5c2b703", "AQAAAAIAAYagAAAAEE5EZxOSjU5K2mK4KfhrHQVOlHF8Aa+fXR+DLxu9ybJ8Ka0yaQUEWj7OrGL1gi7GRQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d43856a7-c75f-4574-9589-90a73c1d0a65", "AQAAAAIAAYagAAAAEPPv1jYinwjsPwXrhQPpHVs9yCGylLKFhduwjT+mVfAhm2Fq6XQBbDJeARaBnrsOAw==" });
        }
    }
}
