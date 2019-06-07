using Microsoft.EntityFrameworkCore.Migrations;

namespace iChat.Api.Migrations
{
    public partial class RenameConversationUserToConversationUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationUser_Conversations_ConversationId",
                table: "ConversationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversationUser_Users_UserId",
                table: "ConversationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConversationUser",
                table: "ConversationUser");

            migrationBuilder.RenameTable(
                name: "ConversationUser",
                newName: "ConversationUsers");

            migrationBuilder.RenameIndex(
                name: "IX_ConversationUser_UserId",
                table: "ConversationUsers",
                newName: "IX_ConversationUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConversationUsers",
                table: "ConversationUsers",
                columns: new[] { "ConversationId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationUsers_Conversations_ConversationId",
                table: "ConversationUsers",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationUsers_Users_UserId",
                table: "ConversationUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationUsers_Conversations_ConversationId",
                table: "ConversationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversationUsers_Users_UserId",
                table: "ConversationUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConversationUsers",
                table: "ConversationUsers");

            migrationBuilder.RenameTable(
                name: "ConversationUsers",
                newName: "ConversationUser");

            migrationBuilder.RenameIndex(
                name: "IX_ConversationUsers_UserId",
                table: "ConversationUser",
                newName: "IX_ConversationUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConversationUser",
                table: "ConversationUser",
                columns: new[] { "ConversationId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationUser_Conversations_ConversationId",
                table: "ConversationUser",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationUser_Users_UserId",
                table: "ConversationUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
