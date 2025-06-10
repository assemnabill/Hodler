using Hodler.Integration.Repositories.Portfolios.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hodler.Integration.Repositories.Portfolios.Context;

public class PortfolioDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Portfolio> Portfolios => Set<Portfolio>();
    public DbSet<ManualTransaction> Transactions => Set<ManualTransaction>();
    public DbSet<BitcoinWallet> BitcoinWallets => Set<BitcoinWallet>();
    public DbSet<BlockchainTransaction> BlockchainTransactions => Set<BlockchainTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PortfolioConfiguration());
        modelBuilder.ApplyConfiguration(new ManualTransactionConfiguration());
        modelBuilder.ApplyConfiguration(new BitcoinWalletConfiguration());
        modelBuilder.ApplyConfiguration(new BlockchainTransactionConfiguration());
    }
}