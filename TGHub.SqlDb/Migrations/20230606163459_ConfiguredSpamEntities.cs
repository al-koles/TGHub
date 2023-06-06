using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGHub.SqlDb.Migrations
{
    public partial class ConfiguredSpamEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BannedUser");

            migrationBuilder.DropTable(
                name: "BannWord");

            migrationBuilder.DropTable(
                name: "ChannelBannTopic");

            migrationBuilder.DropTable(
                name: "BannTopic");

            migrationBuilder.RenameColumn(
                name: "SpamOn",
                table: "Channel",
                newName: "OffensiveSpamOn");

            migrationBuilder.AddColumn<bool>(
                name: "ListSpamOn",
                table: "Channel",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BannUser",
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpamMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorTelegramId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTimeWritten = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpamMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpamMessage_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpamWord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpamWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpamWord_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArchiveBann",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    InitiatorId = table.Column<int>(type: "int", nullable: true),
                    UnBannInitiatorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchiveBann", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchiveBann_BannUser_UserId",
                        column: x => x.UserId,
                        principalTable: "BannUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArchiveBann_ChannelAdministrator_InitiatorId",
                        column: x => x.InitiatorId,
                        principalTable: "ChannelAdministrator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArchiveBann_ChannelAdministrator_UnBannInitiatorId",
                        column: x => x.UnBannInitiatorId,
                        principalTable: "ChannelAdministrator",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchiveBann_InitiatorId",
                table: "ArchiveBann",
                column: "InitiatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchiveBann_UnBannInitiatorId",
                table: "ArchiveBann",
                column: "UnBannInitiatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchiveBann_UserId",
                table: "ArchiveBann",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BannUser_BannInitiatorId",
                table: "BannUser",
                column: "BannInitiatorId");

            migrationBuilder.CreateIndex(
                name: "IX_BannUser_ChannelId_TelegramId",
                table: "BannUser",
                columns: new[] { "ChannelId", "TelegramId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpamMessage_ChannelId_AuthorTelegramId",
                table: "SpamMessage",
                columns: new[] { "ChannelId", "AuthorTelegramId" });

            migrationBuilder.CreateIndex(
                name: "IX_SpamWord_ChannelId",
                table: "SpamWord",
                column: "ChannelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchiveBann");

            migrationBuilder.DropTable(
                name: "SpamMessage");

            migrationBuilder.DropTable(
                name: "SpamWord");

            migrationBuilder.DropTable(
                name: "BannUser");

            migrationBuilder.DropColumn(
                name: "ListSpamOn",
                table: "Channel");

            migrationBuilder.RenameColumn(
                name: "OffensiveSpamOn",
                table: "Channel",
                newName: "SpamOn");

            migrationBuilder.CreateTable(
                name: "BannedUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    BannDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BannTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelegramId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannedUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannedUser_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BannTopic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannTopic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BannWord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannWord_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChannelBannTopic",
                columns: table => new
                {
                    BannTopicsId = table.Column<int>(type: "int", nullable: false),
                    ChannelsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelBannTopic", x => new { x.BannTopicsId, x.ChannelsId });
                    table.ForeignKey(
                        name: "FK_ChannelBannTopic_BannTopic_BannTopicsId",
                        column: x => x.BannTopicsId,
                        principalTable: "BannTopic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelBannTopic_Channel_ChannelsId",
                        column: x => x.ChannelsId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BannedUser_ChannelId_TelegramId",
                table: "BannedUser",
                columns: new[] { "ChannelId", "TelegramId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BannWord_ChannelId",
                table: "BannWord",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelBannTopic_ChannelsId",
                table: "ChannelBannTopic",
                column: "ChannelsId");
        }
    }
}
