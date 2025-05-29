using System.Diagnostics;
using System.Reflection;
using Corz.Extensions.DateTime;
using Hodler.Domain.BitcoinPrices.Services;
using Hodler.Domain.Shared.Models;
using Hodler.Integration.Repositories.BitcoinPrices.Context;
using Hodler.Integration.Repositories.Portfolios.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hodler.Integration.Repositories.Migrations;

internal class HodlerDbInitializer(
    IServiceProvider serviceProvider,
    ILogger<HodlerDbInitializer> logger
) : BackgroundService
{
    private const string ActivitySourceName = "Migrations";
    private readonly ActivitySource _activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await InitializeHodlerDatabaseAsync(cancellationToken);
        await InitializeBitcoinPricesAsync(cancellationToken);
    }

    private async Task InitializeHodlerDatabaseAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContexts = Assembly
            .GetAssembly(typeof(PortfolioDbContext))?
            .GetTypes()
            .Where(t => typeof(DbContext).IsAssignableFrom(t) && !t.IsAbstract)
            .Select(t => scope.ServiceProvider.GetRequiredService(t) as DbContext)
            .OfType<DbContext>()
            .ToList() ?? [];

        using var activity = _activitySource.StartActivity(ActivityKind.Client);

        if (dbContexts.Count == 0)
        {
            logger.LogWarning("No DbContext types were found to migrate.");
            return;
        }

        var sw = Stopwatch.StartNew();
        foreach (var dbContext in dbContexts)
        {
            var contextName = dbContext.GetType().Name;
            logger.LogInformation("Migrating database for context: ({ContextName})", contextName);

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(dbContext.Database.MigrateAsync, cancellationToken);

            logger.LogInformation("Database migration completed for context: ({ContextName})", contextName);
        }

        logger.LogInformation("Hodler Database initialization completed after ({ElapsedMilliseconds}ms)", sw.ElapsedMilliseconds);
    }

    private async Task InitializeBitcoinPricesAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();

            var bitcoinPriceDbContext = scope.ServiceProvider.GetRequiredService<BitcoinPriceDbContext>();
            var bitcoinPriceSyncService = scope.ServiceProvider.GetRequiredService<IBitcoinPriceSyncService>();

            using var activity = _activitySource.StartActivity(ActivityKind.Client);

            var sw = Stopwatch.StartNew();

            var latestPrice = await bitcoinPriceDbContext
                .BitcoinPrices
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync(cancellationToken);

            const int syncPeriodInYears = 10;
            if (latestPrice == null)
                logger.LogInformation("No historical bitcoin price data available. Syncing prices of last {Years} years.", syncPeriodInYears);

            var fromDate = latestPrice?.Date ?? DateTime.UtcNow.AddYears(-syncPeriodInYears).ToDate();
            var toDate = DateTime.UtcNow.ToDate();

            if (fromDate == toDate)
            {
                logger.LogInformation("Bitcoin prices are already up to date.");
                return;
            }

            var bitcoinPrices = await bitcoinPriceSyncService
                .SyncMissingPricesAsync(FiatCurrency.UsDollar, fromDate, toDate, cancellationToken);

            logger.LogInformation(
                "Stored prices from ({From}) to ({To}) in USD.",
                bitcoinPrices.Min(x => x.Date),
                bitcoinPrices.Max(x => x.Date)
            );

            logger.LogInformation("Bitcoin prices sync completed after {Time}", sw.Elapsed);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while syncing bitcoin prices to database");
        }
    }
}