using Microsoft.EntityFrameworkCore.Migrations;

namespace RestIT.Migrations
{
    public partial class rest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Chef_ChefID",
                table: "Restaurant");

            migrationBuilder.AlterColumn<int>(
                name: "ChefID",
                table: "Restaurant",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "selectedChef",
                table: "Restaurant",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Chef_ChefID",
                table: "Restaurant",
                column: "ChefID",
                principalTable: "Chef",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Chef_ChefID",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "selectedChef",
                table: "Restaurant");

            migrationBuilder.AlterColumn<int>(
                name: "ChefID",
                table: "Restaurant",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Chef_ChefID",
                table: "Restaurant",
                column: "ChefID",
                principalTable: "Chef",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
