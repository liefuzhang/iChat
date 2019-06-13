using Microsoft.EntityFrameworkCore.Migrations;

namespace iChat.Api.Migrations
{
    public partial class AddNameToFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SavedName",
                table: "Files",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SavedName",
                table: "Files");
        }
    }
}
