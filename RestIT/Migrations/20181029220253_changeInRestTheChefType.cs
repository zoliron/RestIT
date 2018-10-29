using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RestIT.Migrations
{
    public partial class changeInRestTheChefType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RestaurantChef",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    restaurentID = table.Column<int>(nullable: true),
                    chefID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantChef", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RestaurantChef_Chef_chefID",
                        column: x => x.chefID,
                        principalTable: "Chef",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RestaurantChef_Restaurant_restaurentID",
                        column: x => x.restaurentID,
                        principalTable: "Restaurant",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantChef_chefID",
                table: "RestaurantChef",
                column: "chefID");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantChef_restaurentID",
                table: "RestaurantChef",
                column: "restaurentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantChef");
        }
    }
}
