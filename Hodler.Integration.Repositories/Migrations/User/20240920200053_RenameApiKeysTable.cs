using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hodler.Integration.Repositories.Migrations.User
{
    /// <inheritdoc />
    public partial class RenameApiKeysTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiKey_AspNetUsers_UserId",
                table: "ApiKey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiKey",
                table: "ApiKey");

            migrationBuilder.RenameTable(
                name: "ApiKey",
                newName: "ApiKeys");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "ApiKeys",
                newName: "Value");

            migrationBuilder.RenameIndex(
                name: "IX_ApiKey_UserId",
                table: "ApiKeys",
                newName: "IX_ApiKeys_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiKeys",
                table: "ApiKeys",
                column: "ApiKeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiKeys_AspNetUsers_UserId",
                table: "ApiKeys",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiKeys_AspNetUsers_UserId",
                table: "ApiKeys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiKeys",
                table: "ApiKeys");

            migrationBuilder.RenameTable(
                name: "ApiKeys",
                newName: "ApiKey");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ApiKey",
                newName: "Key");

            migrationBuilder.RenameIndex(
                name: "IX_ApiKeys_UserId",
                table: "ApiKey",
                newName: "IX_ApiKey_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiKey",
                table: "ApiKey",
                column: "ApiKeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiKey_AspNetUsers_UserId",
                table: "ApiKey",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}