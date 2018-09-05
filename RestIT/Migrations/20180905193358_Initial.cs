using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RestIT.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chef",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    chefName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chef", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    custName = table.Column<string>(nullable: true),
                    custPhone = table.Column<string>(nullable: true),
                    custMail = table.Column<string>(nullable: true),
                    custAge = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Restaurant",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    restName = table.Column<string>(nullable: true),
                    restLocation = table.Column<string>(nullable: true),
                    restRating = table.Column<float>(nullable: false),
                    restType = table.Column<string>(nullable: true),
                    restKosher = table.Column<bool>(nullable: false),
                    ChefID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Restaurant_Chef_ChefID",
                        column: x => x.ChefID,
                        principalTable: "Chef",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dish",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    dishName = table.Column<string>(nullable: true),
                    dishCost = table.Column<int>(nullable: false),
                    dishRating = table.Column<float>(nullable: false),
                    dishType = table.Column<string>(nullable: true),
                    RestaurantID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dish", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dish_Restaurant_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurant",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dish_RestaurantID",
                table: "Dish",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_ChefID",
                table: "Restaurant",
                column: "ChefID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Dish");

            migrationBuilder.DropTable(
                name: "Restaurant");

            migrationBuilder.DropTable(
                name: "Chef");
        }
    }
}
