using Microsoft.EntityFrameworkCore.Migrations;

namespace iChat.Api.Migrations
{
    public partial class RemoveFileMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageFileAttachments_Messages_FileMessageId",
                table: "MessageFileAttachments");

            migrationBuilder.RenameColumn(
                name: "FileMessageId",
                table: "MessageFileAttachments",
                newName: "MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageFileAttachments_FileMessageId",
                table: "MessageFileAttachments",
                newName: "IX_MessageFileAttachments_MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageFileAttachments_Messages_MessageId",
                table: "MessageFileAttachments",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageFileAttachments_Messages_MessageId",
                table: "MessageFileAttachments");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "MessageFileAttachments",
                newName: "FileMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageFileAttachments_MessageId",
                table: "MessageFileAttachments",
                newName: "IX_MessageFileAttachments_FileMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageFileAttachments_Messages_FileMessageId",
                table: "MessageFileAttachments",
                column: "FileMessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
