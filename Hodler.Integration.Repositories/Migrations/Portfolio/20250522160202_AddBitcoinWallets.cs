#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Hodler.Integration.Repositories.Migrations.Portfolio
{
    /// <inheritdoc />
    public partial class AddBitcoinWallets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CryptoExchange",
                table: "Transactions");

            migrationBuilder.AddColumn<decimal>(
                name: "Fee",
                table: "Transactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SourceIdentifier",
                table: "Transactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceType",
                table: "Transactions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BitcoinWallets",
                columns: table => new
                {
                    BitcoinWalletId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    PortfolioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    WalletName = table.Column<string>(type: "text", nullable: false),
                    Network = table.Column<int>(type: "integer", nullable: false),
                    ConnectedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastSynced = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinWallets", x => x.BitcoinWalletId);
                    table.ForeignKey(
                        name: "FK_BitcoinWallets_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "PortfolioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinWallets_Address",
                table: "BitcoinWallets",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinWallets_PortfolioId",
                table: "BitcoinWallets",
                column: "PortfolioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BitcoinWallets");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SourceIdentifier",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "CryptoExchange",
                table: "Transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}