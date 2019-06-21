using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreLearnExample.CustMigrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Describe2",
                table: "TableTest");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Describe2",
                table: "TableTest",
                maxLength: 50,
                nullable: true);
        }
    }
}
