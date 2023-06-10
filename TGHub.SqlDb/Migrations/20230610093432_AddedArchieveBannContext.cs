using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGHub.SqlDb.Migrations
{
    public partial class AddedArchieveBannContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Context",
                table: "ArchiveBann",
                newName: "UnBannContext");

            migrationBuilder.AddColumn<string>(
                name: "BannContext",
                table: "ArchiveBann",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannContext",
                table: "ArchiveBann");

            migrationBuilder.RenameColumn(
                name: "UnBannContext",
                table: "ArchiveBann",
                newName: "Context");
        }
    }
}
