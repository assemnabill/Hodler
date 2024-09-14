#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Hodler.Integration.Repositories.Migrations.Portfolio
{
    /// <inheritdoc />
    public partial class AddSysPropertiesToPortfolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Portfolios",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Portfolios",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Portfolios",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "Portfolios",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Portfolios");
        }
    }
}
