using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PUS.Migrations
{
    public partial class mssqllocal_migration_876 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chates_AspNetUsers_ClientId",
                table: "Chates");

            migrationBuilder.DropForeignKey(
                name: "FK_Chates_Services_ServiceId",
                table: "Chates");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatLines_Chates_ChatId",
                table: "ChatLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chates",
                table: "Chates");

            migrationBuilder.RenameTable(
                name: "Chates",
                newName: "Chats");

            migrationBuilder.RenameIndex(
                name: "IX_Chates_ServiceId",
                table: "Chats",
                newName: "IX_Chats_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Chates_ClientId",
                table: "Chats",
                newName: "IX_Chats_ClientId");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "Chats",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "Chats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chats",
                table: "Chats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatLines_Chats_ChatId",
                table: "ChatLines",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_ClientId",
                table: "Chats",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Services_ServiceId",
                table: "Chats",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatLines_Chats_ChatId",
                table: "ChatLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_ClientId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Services_ServiceId",
                table: "Chats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chats",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Chats");

            migrationBuilder.RenameTable(
                name: "Chats",
                newName: "Chates");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_ServiceId",
                table: "Chates",
                newName: "IX_Chates_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_ClientId",
                table: "Chates",
                newName: "IX_Chates_ClientId");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "Chates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chates",
                table: "Chates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chates_AspNetUsers_ClientId",
                table: "Chates",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chates_Services_ServiceId",
                table: "Chates",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatLines_Chates_ChatId",
                table: "ChatLines",
                column: "ChatId",
                principalTable: "Chates",
                principalColumn: "Id");
        }
    }
}
