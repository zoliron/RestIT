using Microsoft.EntityFrameworkCore.Migrations;

namespace RestIT.Migrations
{
    public partial class MantToMany_RestChef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_RestaurantChef_ID",
                table: "RestaurantChef");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "RestaurantChef");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "RestaurantChef",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RestaurantChef_ID",
                table: "RestaurantChef",
                column: "ID");
        }
    }
}
