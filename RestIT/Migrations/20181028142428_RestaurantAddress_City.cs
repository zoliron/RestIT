using Microsoft.EntityFrameworkCore.Migrations;

namespace RestIT.Migrations
{
    public partial class RestaurantAddress_City : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "restLocation",
                table: "Restaurant",
                newName: "restCity");

            migrationBuilder.AddColumn<string>(
                name: "restAddress",
                table: "Restaurant",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "restAddress",
                table: "Restaurant");

            migrationBuilder.RenameColumn(
                name: "restCity",
                table: "Restaurant",
                newName: "restLocation");
        }
    }
}
