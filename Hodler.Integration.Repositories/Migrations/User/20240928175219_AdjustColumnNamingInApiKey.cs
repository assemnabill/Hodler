using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hodler.Integration.Repositories.Migrations.User
{
    /// <inheritdoc />
    public partial class AdjustColumnNamingInApiKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApiName",
                table: "ApiKeys",
                newName: "ApiKeyName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApiKeyName",
                table: "ApiKeys",
                newName: "ApiName");
        }
    }
}
