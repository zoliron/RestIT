using Microsoft.EntityFrameworkCore.Migrations;

namespace RestIT.Migrations
{
    public partial class rest2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "selectedChef",
                table: "Restaurant");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "selectedChef",
                table: "Restaurant",
                nullable: false,
                defaultValue: "");
        }
    }
}
