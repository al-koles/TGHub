using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGHub.SqlDb.Migrations
{
    public partial class AddedDeleteBehaviors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BannedUser_Channel_ChannelId",
                table: "BannedUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BannWord_Channel_ChannelId",
                table: "BannWord");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelAdministrator_Channel_ChannelId",
                table: "ChannelAdministrator");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelAdministrator_User_AdministratorId",
                table: "ChannelAdministrator");

            migrationBuilder.DropForeignKey(
                name: "FK_Lottery_ChannelAdministrator_CreatorId",
                table: "Lottery");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_ChannelAdministrator_CreatorId",
                table: "Post");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Channel",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_BannedUser_Channel_ChannelId",
                table: "BannedUser",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BannWord_Channel_ChannelId",
                table: "BannWord",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelAdministrator_Channel_ChannelId",
                table: "ChannelAdministrator",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelAdministrator_User_AdministratorId",
                table: "ChannelAdministrator",
                column: "AdministratorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lottery_ChannelAdministrator_CreatorId",
                table: "Lottery",
                column: "CreatorId",
                principalTable: "ChannelAdministrator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_ChannelAdministrator_CreatorId",
                table: "Post",
                column: "CreatorId",
                principalTable: "ChannelAdministrator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BannedUser_Channel_ChannelId",
                table: "BannedUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BannWord_Channel_ChannelId",
                table: "BannWord");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelAdministrator_Channel_ChannelId",
                table: "ChannelAdministrator");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelAdministrator_User_AdministratorId",
                table: "ChannelAdministrator");

            migrationBuilder.DropForeignKey(
                name: "FK_Lottery_ChannelAdministrator_CreatorId",
                table: "Lottery");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_ChannelAdministrator_CreatorId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Channel");

            migrationBuilder.AddForeignKey(
                name: "FK_BannedUser_Channel_ChannelId",
                table: "BannedUser",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BannWord_Channel_ChannelId",
                table: "BannWord",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelAdministrator_Channel_ChannelId",
                table: "ChannelAdministrator",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelAdministrator_User_AdministratorId",
                table: "ChannelAdministrator",
                column: "AdministratorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lottery_ChannelAdministrator_CreatorId",
                table: "Lottery",
                column: "CreatorId",
                principalTable: "ChannelAdministrator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_ChannelAdministrator_CreatorId",
                table: "Post",
                column: "CreatorId",
                principalTable: "ChannelAdministrator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
