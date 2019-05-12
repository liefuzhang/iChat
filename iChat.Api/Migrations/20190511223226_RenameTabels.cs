using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iChat.Api.Migrations
{
    public partial class RenameTabels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_WorkSpace_WorkSpaceId",
                table: "Channel");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelSubscription_Channel_ChannelId",
                table: "ChannelSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelSubscription_User_UserId",
                table: "ChannelSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Channel_ChannelId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_ReceiverId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_SenderId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_User_WorkSpace_WorkSpaceId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkSpace",
                table: "WorkSpace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChannelSubscription",
                table: "ChannelSubscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Channel",
                table: "Channel");

            migrationBuilder.RenameTable(
                name: "WorkSpace",
                newName: "Workspaces");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "ChannelSubscription",
                newName: "ChannelSubscriptions");

            migrationBuilder.RenameTable(
                name: "Channel",
                newName: "Channels");

            migrationBuilder.RenameColumn(
                name: "WorkSpaceId",
                table: "Users",
                newName: "WorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_User_WorkSpaceId",
                table: "Users",
                newName: "IX_Users_WorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_SenderId",
                table: "Messages",
                newName: "IX_Messages_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_ReceiverId",
                table: "Messages",
                newName: "IX_Messages_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_ChannelId",
                table: "Messages",
                newName: "IX_Messages_ChannelId");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelSubscription_UserId",
                table: "ChannelSubscriptions",
                newName: "IX_ChannelSubscriptions_UserId");

            migrationBuilder.RenameColumn(
                name: "WorkSpaceId",
                table: "Channels",
                newName: "WorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Channel_WorkSpaceId",
                table: "Channels",
                newName: "IX_Channels_WorkspaceId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workspaces",
                table: "Workspaces",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChannelSubscriptions",
                table: "ChannelSubscriptions",
                columns: new[] { "ChannelId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channels",
                table: "Channels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Workspaces_WorkspaceId",
                table: "Channels",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelSubscriptions_Channels_ChannelId",
                table: "ChannelSubscriptions",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelSubscriptions_Users_UserId",
                table: "ChannelSubscriptions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Channels_ChannelId",
                table: "Messages",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_ReceiverId",
                table: "Messages",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Workspaces_WorkspaceId",
                table: "Users",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Workspaces_WorkspaceId",
                table: "Channels");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelSubscriptions_Channels_ChannelId",
                table: "ChannelSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelSubscriptions_Users_UserId",
                table: "ChannelSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Channels_ChannelId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_ReceiverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Workspaces_WorkspaceId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workspaces",
                table: "Workspaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChannelSubscriptions",
                table: "ChannelSubscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Channels",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Workspaces",
                newName: "WorkSpace");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Message");

            migrationBuilder.RenameTable(
                name: "ChannelSubscriptions",
                newName: "ChannelSubscription");

            migrationBuilder.RenameTable(
                name: "Channels",
                newName: "Channel");

            migrationBuilder.RenameColumn(
                name: "WorkspaceId",
                table: "User",
                newName: "WorkSpaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_WorkspaceId",
                table: "User",
                newName: "IX_User_WorkSpaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SenderId",
                table: "Message",
                newName: "IX_Message_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ReceiverId",
                table: "Message",
                newName: "IX_Message_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ChannelId",
                table: "Message",
                newName: "IX_Message_ChannelId");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelSubscriptions_UserId",
                table: "ChannelSubscription",
                newName: "IX_ChannelSubscription_UserId");

            migrationBuilder.RenameColumn(
                name: "WorkspaceId",
                table: "Channel",
                newName: "WorkSpaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Channels_WorkspaceId",
                table: "Channel",
                newName: "IX_Channel_WorkSpaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkSpace",
                table: "WorkSpace",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChannelSubscription",
                table: "ChannelSubscription",
                columns: new[] { "ChannelId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channel",
                table: "Channel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_WorkSpace_WorkSpaceId",
                table: "Channel",
                column: "WorkSpaceId",
                principalTable: "WorkSpace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelSubscription_Channel_ChannelId",
                table: "ChannelSubscription",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelSubscription_User_UserId",
                table: "ChannelSubscription",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Channel_ChannelId",
                table: "Message",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_User_WorkSpace_WorkSpaceId",
                table: "User",
                column: "WorkSpaceId",
                principalTable: "WorkSpace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
