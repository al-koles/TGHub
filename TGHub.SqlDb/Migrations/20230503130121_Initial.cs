using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGHub.SqlDb.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Channel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TelegramId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpamOn = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TelegramId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BannedUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TelegramId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BannDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BannTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChannelId = table.Column<int>(type: "int", nullable: false)
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
                name: "BannWord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannWord_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "ChannelAdministrator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AdministratorId = table.Column<int>(type: "int", nullable: false),
                    ChannelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelAdministrator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelAdministrator_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelAdministrator_User_AdministratorId",
                        column: x => x.AdministratorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lottery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WinnersCount = table.Column<int>(type: "int", nullable: false),
                    TelegramId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lottery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lottery_ChannelAdministrator_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "ChannelAdministrator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TelegramId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_ChannelAdministrator_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "ChannelAdministrator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LotteryAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LotteryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotteryAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LotteryAttachment_Lottery_LotteryId",
                        column: x => x.LotteryId,
                        principalTable: "Lottery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LotteryParticipant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TelegramId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsWinner = table.Column<bool>(type: "bit", nullable: false),
                    LotteryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotteryParticipant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LotteryParticipant_Lottery_LotteryId",
                        column: x => x.LotteryId,
                        principalTable: "Lottery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostAttachment_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostButton",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostButton", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostButton_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
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
                name: "IX_Channel_TelegramId",
                table: "Channel",
                column: "TelegramId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelAdministrator_AdministratorId",
                table: "ChannelAdministrator",
                column: "AdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelAdministrator_ChannelId_AdministratorId",
                table: "ChannelAdministrator",
                columns: new[] { "ChannelId", "AdministratorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelBannTopic_ChannelsId",
                table: "ChannelBannTopic",
                column: "ChannelsId");

            migrationBuilder.CreateIndex(
                name: "IX_Lottery_CreatorId",
                table: "Lottery",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryAttachment_LotteryId",
                table: "LotteryAttachment",
                column: "LotteryId");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryParticipant_LotteryId_TelegramId",
                table: "LotteryParticipant",
                columns: new[] { "LotteryId", "TelegramId" });

            migrationBuilder.CreateIndex(
                name: "IX_Post_CreatorId",
                table: "Post",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PostAttachment_PostId",
                table: "PostAttachment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostButton_PostId",
                table: "PostButton",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_User_TelegramId",
                table: "User",
                column: "TelegramId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BannedUser");

            migrationBuilder.DropTable(
                name: "BannWord");

            migrationBuilder.DropTable(
                name: "ChannelBannTopic");

            migrationBuilder.DropTable(
                name: "LotteryAttachment");

            migrationBuilder.DropTable(
                name: "LotteryParticipant");

            migrationBuilder.DropTable(
                name: "PostAttachment");

            migrationBuilder.DropTable(
                name: "PostButton");

            migrationBuilder.DropTable(
                name: "BannTopic");

            migrationBuilder.DropTable(
                name: "Lottery");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "ChannelAdministrator");

            migrationBuilder.DropTable(
                name: "Channel");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
