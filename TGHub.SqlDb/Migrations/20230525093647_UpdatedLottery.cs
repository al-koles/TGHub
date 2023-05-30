using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGHub.SqlDb.Migrations
{
    public partial class UpdatedLottery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Link",
                table: "LotteryAttachment",
                newName: "FileName");

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "LotteryParticipant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "LotteryAttachment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentsFolderId",
                table: "Lottery",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "AttachmentsFormat",
                table: "Lottery",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NickName",
                table: "LotteryParticipant");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "LotteryAttachment");

            migrationBuilder.DropColumn(
                name: "AttachmentsFolderId",
                table: "Lottery");

            migrationBuilder.DropColumn(
                name: "AttachmentsFormat",
                table: "Lottery");

            migrationBuilder.DropColumn(
                name: "ResultMessage",
                table: "Lottery");

            migrationBuilder.DropColumn(
                name: "VoteButtonContent",
                table: "Lottery");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "LotteryAttachment",
                newName: "Link");
        }
    }
}
