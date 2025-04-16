#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Hodler.Integration.Repositories.Migrations.User
{
    /// <inheritdoc />
    public partial class AddCurrencyAndThemeSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "UserSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Theme",
                table: "UserSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "Theme",
                table: "UserSettings");
        }
    }
}