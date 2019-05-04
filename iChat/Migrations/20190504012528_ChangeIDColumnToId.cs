using Microsoft.EntityFrameworkCore.Migrations;

namespace iChat.Migrations
{
    public partial class ChangeIDColumnToId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChannelId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ChannelId",
                table: "User",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Channel_ChannelId",
                table: "User",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Channel_ChannelId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ChannelId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "User");
        }
    }
}
