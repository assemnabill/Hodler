using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hodler.Integration.Repositories.Migrations.Portfolio
{
    /// <inheritdoc />
    public partial class AddTransactionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Transactions",
                newName: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Transactions",
                newName: "Id");
        }
    }
}
