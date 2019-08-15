using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreLearnExample.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TableTest",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Introduce = table.Column<string>(maxLength: 50, nullable: true),
                    Describe = table.Column<string>(maxLength: 50, nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTest", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableTest");
        }
    }
}
