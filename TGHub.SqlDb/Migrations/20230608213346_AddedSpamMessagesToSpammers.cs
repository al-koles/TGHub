using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGHub.SqlDb.Migrations
{
    public partial class AddedSpamMessagesToSpammers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchiveBann_BannUser_UserId",
                table: "ArchiveBann");

            migrationBuilder.DropForeignKey(
                name: "FK_SpamMessage_Channel_ChannelId",
                table: "SpamMessage");

            migrationBuilder.DropTable(
                name: "BannUser");

            migrationBuilder.DropColumn(
                name: "AuthorTelegramId",
                table: "SpamMessage");

            migrationBuilder.RenameColumn(
                name: "ChannelId",
                table: "SpamMessage",
                newName: "SpammerId");

            migrationBuilder.RenameIndex(
                name: "IX_SpamMessage_ChannelId_TelegramId",
                table: "SpamMessage",
                newName: "IX_SpamMessage_SpammerId_TelegramId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ArchiveBann",
                newName: "SpammerId");

            migrationBuilder.RenameIndex(
                name: "IX_ArchiveBann_UserId",
                table: "ArchiveBann",
                newName: "IX_ArchiveBann_SpammerId");

            migrationBuilder.CreateTable(
                name: "Spammer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TelegramId = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BannDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BannContext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    BannInitiatorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spammer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spammer_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Spammer_ChannelAdministrator_BannInitiatorId",
                        column: x => x.BannInitiatorId,
                        principalTable: "ChannelAdministrator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Spammer_BannInitiatorId",
                table: "Spammer",
                column: "BannInitiatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Spammer_ChannelId_TelegramId",
                table: "Spammer",
                columns: new[] { "ChannelId", "TelegramId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ArchiveBann_Spammer_SpammerId",
                table: "ArchiveBann",
                column: "SpammerId",
                principalTable: "Spammer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpamMessage_Spammer_SpammerId",
                table: "SpamMessage",
                column: "SpammerId",
                principalTable: "Spammer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchiveBann_Spammer_SpammerId",
                table: "ArchiveBann");

            migrationBuilder.DropForeignKey(
                name: "FK_SpamMessage_Spammer_SpammerId",
                table: "SpamMessage");

            migrationBuilder.DropTable(
                name: "Spammer");

            migrationBuilder.RenameColumn(
                name: "SpammerId",
                table: "SpamMessage",
                newName: "ChannelId");

            migrationBuilder.RenameIndex(
                name: "IX_SpamMessage_SpammerId_TelegramId",
                table: "SpamMessage",
                newName: "IX_SpamMessage_ChannelId_TelegramId");

            migrationBuilder.RenameColumn(
                name: "SpammerId",
                table: "ArchiveBann",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ArchiveBann_SpammerId",
                table: "ArchiveBann",
                newName: "IX_ArchiveBann_UserId");

            migrationBuilder.AddColumn<long>(
                name: "AuthorTelegramId",
                table: "SpamMessage",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "BannUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannInitiatorId = table.Column<int>(type: "int", nullable: true),
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    BannContext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BannDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelegramId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannUser_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannUser_ChannelAdministrator_BannInitiatorId",
                        column: x => x.BannInitiatorId,
                        principalTable: "ChannelAdministrator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BannUser_BannInitiatorId",
                table: "BannUser",
                column: "BannInitiatorId");

            migrationBuilder.CreateIndex(
                name: "IX_BannUser_ChannelId_TelegramId",
                table: "BannUser",
                columns: new[] { "ChannelId", "TelegramId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ArchiveBann_BannUser_UserId",
                table: "ArchiveBann",
                column: "UserId",
                principalTable: "BannUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpamMessage_Channel_ChannelId",
                table: "SpamMessage",
                column: "ChannelId",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
