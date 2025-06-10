using Hodler.Integration.Repositories.Portfolios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hodler.Integration.Repositories.Portfolios.Context;

internal class PortfolioConfiguration : IEntityTypeConfiguration<Portfolio>
{
    public void Configure(EntityTypeBuilder<Portfolio> builder)
    {

        builder
            .Property(x => x.PortfolioId)
            .ValueGeneratedNever()
            .HasDefaultValueSql(SqlFunctions.NewId);

        builder
            .HasMany(x => x.ManualTransactions)
            .WithOne(x => x.Portfolio)
            .HasForeignKey(x => x.PortfolioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.BitcoinWallets)
            .WithOne(x => x.Portfolio)
            .HasForeignKey(x => x.PortfolioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}