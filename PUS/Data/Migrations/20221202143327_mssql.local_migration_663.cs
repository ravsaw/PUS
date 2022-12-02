using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PUS.Data.Migrations
{
    public partial class mssqllocal_migration_663 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_AspNetUsers_UserId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_UserId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "UserForeignKey",
                table: "Services",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_UserForeignKey",
                table: "Services",
                column: "UserForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_AspNetUsers_UserForeignKey",
                table: "Services",
                column: "UserForeignKey",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_AspNetUsers_UserForeignKey",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_UserForeignKey",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UserForeignKey",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Services",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Services_UserId",
                table: "Services",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_AspNetUsers_UserId",
                table: "Services",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
