using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGHub.SqlDb.Migrations
{
    public partial class ReplaceChannelPhotoUrlWithLogoFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "Channel",
                newName: "LogoFileName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LogoFileName",
                table: "Channel",
                newName: "PhotoUrl");
        }
    }
}
