// <autogenerated />
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pumpkin.Beer.Taste.Migrations
{
    /// <inheritdoc />
    public partial class Photo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.RenameTable(
                name: "UserInvite",
                newName: "UserInvite",
                newSchema: "app");

            migrationBuilder.RenameTable(
                name: "BlindVote",
                newName: "BlindVote",
                newSchema: "app");

            migrationBuilder.RenameTable(
                name: "BlindItem",
                newName: "BlindItem",
                newSchema: "app");

            migrationBuilder.RenameTable(
                name: "Blind",
                newName: "Blind",
                newSchema: "app");

            migrationBuilder.AddColumn<byte[]>(
                name: "CoverPhoto",
                schema: "app",
                table: "Blind",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverPhoto",
                schema: "app",
                table: "Blind");

            migrationBuilder.RenameTable(
                name: "UserInvite",
                schema: "app",
                newName: "UserInvite");

            migrationBuilder.RenameTable(
                name: "BlindVote",
                schema: "app",
                newName: "BlindVote");

            migrationBuilder.RenameTable(
                name: "BlindItem",
                schema: "app",
                newName: "BlindItem");

            migrationBuilder.RenameTable(
                name: "Blind",
                schema: "app",
                newName: "Blind");
        }
    }
}