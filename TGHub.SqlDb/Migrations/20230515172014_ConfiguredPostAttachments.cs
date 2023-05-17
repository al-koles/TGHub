using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGHub.SqlDb.Migrations
{
    public partial class ConfiguredPostAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Link",
                table: "PostAttachment",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "TelegramId",
                table: "Lottery",
                newName: "ResultTelegramId");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "PostAttachment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "LotteryTelegramId",
                table: "Lottery",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "PostAttachment");

            migrationBuilder.DropColumn(
                name: "LotteryTelegramId",
                table: "Lottery");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "PostAttachment",
                newName: "Link");

            migrationBuilder.RenameColumn(
                name: "ResultTelegramId",
                table: "Lottery",
                newName: "TelegramId");
        }
    }
}
