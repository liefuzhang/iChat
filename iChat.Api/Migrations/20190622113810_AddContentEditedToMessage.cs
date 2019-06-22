using Microsoft.EntityFrameworkCore.Migrations;

namespace iChat.Api.Migrations
{
    public partial class AddContentEditedToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ContentEdited",
                table: "Messages",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentEdited",
                table: "Messages");
        }
    }
}
