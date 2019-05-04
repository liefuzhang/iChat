using Microsoft.EntityFrameworkCore.Migrations;

namespace iChat.Migrations
{
    public partial class CreateChannelMessageAndDirectMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_UserId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Channel_ChannelId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ChannelId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Message",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_UserId",
                table: "Message",
                newName: "IX_Message_SenderId");

            migrationBuilder.AlterColumn<int>(
                name: "ChannelId",
                table: "Message",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ReceiverId",
                table: "Message",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Message",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ReceiverId",
                table: "Message",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_ReceiverId",
                table: "Message",
                column: "ReceiverId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_SenderId",
                table: "Message",
                column: "SenderId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_ReceiverId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_SenderId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ReceiverId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Message",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                newName: "IX_Message_UserId");

            migrationBuilder.AddColumn<int>(
                name: "ChannelId",
                table: "User",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChannelId",
                table: "Message",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ChannelId",
                table: "User",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_UserId",
                table: "Message",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Channel_ChannelId",
                table: "User",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
