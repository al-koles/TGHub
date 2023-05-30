using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGHub.SqlDb.Migrations
{
    public partial class RenamedLotteryDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultMessage",
                table: "Lottery");

            migrationBuilder.DropColumn(
                name: "VoteButtonContent",
                table: "Lottery");

            migrationBuilder.RenameColumn(
                name: "ToDateTime",
                table: "Lottery",
                newName: "StartDateTime");

            migrationBuilder.RenameColumn(
                name: "FromDateTime",
                table: "Lottery",
                newName: "EndDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "Lottery",
                newName: "ToDateTime");

            migrationBuilder.RenameColumn(
                name: "EndDateTime",
                table: "Lottery",
                newName: "FromDateTime");

            migrationBuilder.AddColumn<string>(
                name: "ResultMessage",
                table: "Lottery",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VoteButtonContent",
                table: "Lottery",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
