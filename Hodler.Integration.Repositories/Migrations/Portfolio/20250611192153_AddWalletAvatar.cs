#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Hodler.Integration.Repositories.Migrations.Portfolio
{
    /// <inheritdoc />
    public partial class AddWalletAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarColor",
                table: "BitcoinWallets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AvatarIcon",
                table: "BitcoinWallets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarColor",
                table: "BitcoinWallets");

            migrationBuilder.DropColumn(
                name: "AvatarIcon",
                table: "BitcoinWallets");
        }
    }
}