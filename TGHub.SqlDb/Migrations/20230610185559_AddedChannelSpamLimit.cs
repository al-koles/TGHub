using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGHub.SqlDb.Migrations
{
    public partial class AddedChannelSpamLimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpamMessageLimit",
                table: "Channel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpamMessageLimit",
                table: "Channel");
        }
    }
}
