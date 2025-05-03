using System.Diagnostics;
using Corz.Extensions.DateTime;
using Hodler.Domain.BitcoinPrices.Services;
using Hodler.Domain.Shared.Models;
using Hodler.Integration.Repositories.BitcoinPrices.Context;
using Hodler.Integration.Repositories.Portfolios.Context;
using Hodler.Integration.Repositories.Users.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hodler.Integration.Repositories.Migrations;

internal class HodlerDbInitializer(IServiceProvider serviceProvider, ILogger<HodlerDbInitializer> logger)
    : BackgroundService
{
    public const string ActivitySourceName = "Migrations";

    private readonly ActivitySource _activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
        await InitializeDatabaseAsync(dbContext, cancellationToken);

        var identityDbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        await InitializeDatabaseAsync(identityDbContext, cancellationToken);

        var bitcoinPriceDbContext = scope.ServiceProvider.GetRequiredService<BitcoinPriceDbContext>();
        var historicalBitcoinPriceProvider = scope.ServiceProvider.GetRequiredService<IBitcoinPriceSyncService>();

        await InitializeDatabaseAsync(
            bitcoinPriceDbContext,
            historicalBitcoinPriceProvider,
            cancellationToken
        );
    }

    private async Task InitializeDatabaseAsync(
        BitcoinPriceDbContext bitcoinPriceDbContext,
        IBitcoinPriceSyncService bitcoinPriceSyncService,
        CancellationToken cancellationToken
    )
    {
        try
        {
            using var activity = _activitySource.StartActivity("Initializing bitcoin price database", ActivityKind.Client);

            var sw = Stopwatch.StartNew();

            var strategy = bitcoinPriceDbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(bitcoinPriceDbContext.Database.MigrateAsync, cancellationToken);

            logger.LogInformation("Database migrations completed. Initializing historical bitcoin price data...");

            var latestPrice = await bitcoinPriceDbContext
                .BitcoinPrices
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync(cancellationToken);

            const int syncPeriodInYears = 10;
            if (latestPrice == null)
                logger.LogInformation("No historical bitcoin price data available. Syncing prices of last {Years} years.", syncPeriodInYears);

            var from = latestPrice?.Date ?? DateTime.UtcNow.AddYears(-syncPeriodInYears).ToDate();
            var to = DateTime.UtcNow.ToDate();

            if (from == to)
            {
                logger.LogInformation("Bitcoin prices are already up to date.");
                return;
            }

            var bitcoinPrices = await bitcoinPriceSyncService
                .SyncMissingPricesAsync(FiatCurrency.UsDollar, from, to, cancellationToken);

            logger.LogInformation(
                "Stored prices from ({From}) to ({To}) in USD.",
                bitcoinPrices.Min(x => x.Date),
                bitcoinPrices.Max(x => x.Date)
            );

            logger.LogInformation("Database initialization completed after {Time}", sw.Elapsed);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while initializing bitcoin price database");
        }
    }

    private async Task InitializeDatabaseAsync(UserDbContext identityDbContext, CancellationToken cancellationToken)
    {
        using var activity = _activitySource.StartActivity("Initializing identity", ActivityKind.Client);

        var sw = Stopwatch.StartNew();

        var strategy = identityDbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(identityDbContext.Database.MigrateAsync, cancellationToken);

        logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms",
            sw.ElapsedMilliseconds);
    }

    private async Task InitializeDatabaseAsync(PortfolioDbContext dbContext, CancellationToken cancellationToken)
    {
        using var activity = _activitySource.StartActivity("Initializing hodler database", ActivityKind.Client);

        var sw = Stopwatch.StartNew();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(dbContext.Database.MigrateAsync, cancellationToken);

        logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms",
            sw.ElapsedMilliseconds);
    }
}