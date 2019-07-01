using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iChat.Api.Migrations
{
    public partial class AddedCreatedInfoForConversationAndChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Conversations",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Conversations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Channels",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Channels",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_CreatedByUserId",
                table: "Conversations",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_CreatedByUserId",
                table: "Channels",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Users_CreatedByUserId",
                table: "Channels",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Users_CreatedByUserId",
                table: "Conversations",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Users_CreatedByUserId",
                table: "Channels");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Users_CreatedByUserId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_CreatedByUserId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Channels_CreatedByUserId",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Channels");
        }
    }
}
