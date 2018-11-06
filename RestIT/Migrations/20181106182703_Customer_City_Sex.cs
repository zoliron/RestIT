using Microsoft.EntityFrameworkCore.Migrations;

namespace RestIT.Migrations
{
    public partial class Customer_City_Sex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "TypeCalc",
                newName: "Type");

            migrationBuilder.AddColumn<int>(
                name: "custCity",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "custSex",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "custCity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "custSex",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "TypeCalc",
                newName: "type");
        }
    }
}
