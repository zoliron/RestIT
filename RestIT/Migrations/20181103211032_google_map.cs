using Microsoft.EntityFrameworkCore.Migrations;

namespace RestIT.Migrations
{
    public partial class google_map : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lng",
                table: "Restaurant",
                newName: "restLng");

            migrationBuilder.RenameColumn(
                name: "Lat",
                table: "Restaurant",
                newName: "restLat");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "restLng",
                table: "Restaurant",
                newName: "Lng");

            migrationBuilder.RenameColumn(
                name: "restLat",
                table: "Restaurant",
                newName: "Lat");
        }
    }
}
