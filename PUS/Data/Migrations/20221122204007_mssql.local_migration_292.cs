using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PUS.Data.Migrations
{
    public partial class mssqllocal_migration_292 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BadLevel",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EQI",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GoodLevel",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "BadLevel",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EQI",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GoodLevel",
                table: "AspNetUsers");
        }
    }
}
