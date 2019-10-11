using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pumpkin.Beer.Taste.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blind",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Started = table.Column<DateTime>(nullable: false),
                    Closed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blind", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlindItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Make = table.Column<string>(nullable: true),
                    BlindId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlindItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlindItem_Blind_BlindId",
                        column: x => x.BlindId,
                        principalTable: "Blind",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlindVote",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    BlindItemId = table.Column<int>(nullable: false),
                    BlindItemOrdinal = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlindVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlindVote_BlindItem_BlindItemId",
                        column: x => x.BlindItemId,
                        principalTable: "BlindItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlindVote_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlindItem_BlindId",
                table: "BlindItem",
                column: "BlindId");

            migrationBuilder.CreateIndex(
                name: "IX_BlindVote_BlindItemId",
                table: "BlindVote",
                column: "BlindItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BlindVote_UserId",
                table: "BlindVote",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlindVote");

            migrationBuilder.DropTable(
                name: "BlindItem");

            migrationBuilder.DropTable(
                name: "Blind");
        }
    }
}
