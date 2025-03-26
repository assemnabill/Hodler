using Microsoft.EntityFrameworkCore;

namespace Hodler.Integration.Repositories.Portfolios.Context;

public class PortfolioDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Entities.Portfolio> Portfolios => Set<Entities.Portfolio>();
    public DbSet<Entities.Transaction> Transactions => Set<Entities.Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PortfolioConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
    }
}