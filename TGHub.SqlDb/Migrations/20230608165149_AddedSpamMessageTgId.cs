using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGHub.SqlDb.Migrations
{
    public partial class AddedSpamMessageTgId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SpamMessage_ChannelId_AuthorTelegramId",
                table: "SpamMessage");

            migrationBuilder.AddColumn<int>(
                name: "TelegramId",
                table: "SpamMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SpamMessage_ChannelId_TelegramId",
                table: "SpamMessage",
                columns: new[] { "ChannelId", "TelegramId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SpamMessage_ChannelId_TelegramId",
                table: "SpamMessage");

            migrationBuilder.DropColumn(
                name: "TelegramId",
                table: "SpamMessage");

            migrationBuilder.CreateIndex(
                name: "IX_SpamMessage_ChannelId_AuthorTelegramId",
                table: "SpamMessage",
                columns: new[] { "ChannelId", "AuthorTelegramId" });
        }
    }
}
