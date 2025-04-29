using Hodler.Integration.Repositories.BitcoinPrices.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hodler.Integration.Repositories.BitcoinPrices.Context;

public class BitcoinPriceDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<BitcoinPrice> BitcoinPrices => Set<BitcoinPrice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new BitcoinPriceConfiguration());
    }
}