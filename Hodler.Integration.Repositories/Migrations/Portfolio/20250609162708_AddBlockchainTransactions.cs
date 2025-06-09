#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Hodler.Integration.Repositories.Migrations.Portfolio
{
    /// <inheritdoc />
    public partial class AddBlockchainTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PortfolioId",
                table: "Portfolios",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "BlockchainTransactions",
                columns: table => new
                {
                    TransactionHash = table.Column<string>(type: "text", nullable: false),
                    PortfolioId = table.Column<Guid>(type: "uuid", nullable: false),
                    BitcoinWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    BtcAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MarketPriceInUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    FiatValueInUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FromAddress = table.Column<string>(type: "text", nullable: false),
                    ToAddress = table.Column<string>(type: "text", nullable: false),
                    NetworkFeeInBtc = table.Column<decimal>(type: "numeric", nullable: false),
                    NetworkFeeInUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockchainTransactions", x => x.TransactionHash);
                    table.ForeignKey(
                        name: "FK_BlockchainTransactions_BitcoinWallets_BitcoinWalletId",
                        column: x => x.BitcoinWalletId,
                        principalTable: "BitcoinWallets",
                        principalColumn: "BitcoinWalletId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlockchainTransactions_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "PortfolioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockchainTransactions_BitcoinWalletId",
                table: "BlockchainTransactions",
                column: "BitcoinWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockchainTransactions_PortfolioId",
                table: "BlockchainTransactions",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockchainTransactions_Timestamp_Type_FiatValueInUsd_BtcAmo~",
                table: "BlockchainTransactions",
                columns: new[] { "Timestamp", "Type", "FiatValueInUsd", "BtcAmount", "FromAddress", "ToAddress", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockchainTransactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "PortfolioId",
                table: "Portfolios",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");
        }
    }
}