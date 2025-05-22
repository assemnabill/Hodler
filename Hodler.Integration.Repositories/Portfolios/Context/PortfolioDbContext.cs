using Hodler.Integration.Repositories.Portfolios.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hodler.Integration.Repositories.Portfolios.Context;

public class PortfolioDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Portfolio> Portfolios => Set<Portfolio>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<BitcoinWallet> BitcoinWallets => Set<BitcoinWallet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PortfolioConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new BitcoinWalletConfiguration());
    }
}