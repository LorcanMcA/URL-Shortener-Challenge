using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace URL_Shortner_Challenge.Data.Migrations
{
    public partial class UpdatedShortLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "ShortLink",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "expired",
                table: "ShortLink",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "ShortLink");

            migrationBuilder.DropColumn(
                name: "expired",
                table: "ShortLink");
        }
    }
}
