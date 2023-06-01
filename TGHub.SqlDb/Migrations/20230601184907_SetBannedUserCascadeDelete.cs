using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGHub.SqlDb.Migrations
{
    public partial class SetBannedUserCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BannedUser_Channel_ChannelId",
                table: "BannedUser");

            migrationBuilder.AddForeignKey(
                name: "FK_BannedUser_Channel_ChannelId",
                table: "BannedUser",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BannedUser_Channel_ChannelId",
                table: "BannedUser");

            migrationBuilder.AddForeignKey(
                name: "FK_BannedUser_Channel_ChannelId",
                table: "BannedUser",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
