using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iChat.Api.Migrations
{
    public partial class AddWorkSpaceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkSpaceId",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkSpaceId",
                table: "Channel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkSpace",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    OwnerId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSpace", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_WorkSpaceId",
                table: "User",
                column: "WorkSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Channel_WorkSpaceId",
                table: "Channel",
                column: "WorkSpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_WorkSpace_WorkSpaceId",
                table: "Channel",
                column: "WorkSpaceId",
                principalTable: "WorkSpace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_WorkSpace_WorkSpaceId",
                table: "User",
                column: "WorkSpaceId",
                principalTable: "WorkSpace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_WorkSpace_WorkSpaceId",
                table: "Channel");

            migrationBuilder.DropForeignKey(
                name: "FK_User_WorkSpace_WorkSpaceId",
                table: "User");

            migrationBuilder.DropTable(
                name: "WorkSpace");

            migrationBuilder.DropIndex(
                name: "IX_User_WorkSpaceId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Channel_WorkSpaceId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "WorkSpaceId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WorkSpaceId",
                table: "Channel");
        }
    }
}
