// <autogenerated />
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pumpkin.Beer.Taste.Migrations
{
    /// <inheritdoc />
    public partial class AddInviteCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InviteCode",
                table: "Blind",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserInvite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlindId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedByUserDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInvite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInvite_Blind_BlindId",
                        column: x => x.BlindId,
                        principalTable: "Blind",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInvite_BlindId",
                table: "UserInvite",
                column: "BlindId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInvite");

            migrationBuilder.DropColumn(
                name: "InviteCode",
                table: "Blind");
        }
    }
}
