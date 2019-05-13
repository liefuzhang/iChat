using Microsoft.EntityFrameworkCore.Migrations;

namespace iChat.Api.Migrations
{
    public partial class AddWorkspaceIdToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkspaceId",
                table: "Messages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Messages");
        }
    }
}
