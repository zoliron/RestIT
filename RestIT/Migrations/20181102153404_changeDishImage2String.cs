using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RestIT.Migrations
{
    public partial class changeDishImage2String : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "dishImage",
                table: "Dish",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "dishImage",
                table: "Dish",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
