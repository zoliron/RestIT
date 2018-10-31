using Microsoft.EntityFrameworkCore.Migrations;

namespace RestIT.Migrations
{
    public partial class inChefchangeRestaurant2RestaurantChef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Chef_ChefID",
                table: "Restaurant");

            migrationBuilder.DropIndex(
                name: "IX_Restaurant_ChefID",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "ChefID",
                table: "Restaurant");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChefID",
                table: "Restaurant",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_ChefID",
                table: "Restaurant",
                column: "ChefID");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Chef_ChefID",
                table: "Restaurant",
                column: "ChefID",
                principalTable: "Chef",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
