using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace URL_Shortner_Challenge.Data.Migrations
{
    public partial class initialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShortLink",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    entered = table.Column<string>(nullable: true),
                    returned = table.Column<string>(nullable: true),
                    created = table.Column<DateTime>(nullable: false),
                    expired = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortLink", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortLink");
        }
    }
}
