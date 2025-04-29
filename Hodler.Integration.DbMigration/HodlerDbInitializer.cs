using System.Diagnostics;
using Hodler.Integration.Repositories.BitcoinPrices.Context;
using Hodler.Integration.Repositories.Portfolios.Context;
using Hodler.Integration.Repositories.Users.Context;
using Microsoft.EntityFrameworkCore;

namespace Hodler.Integration.DbMigration;

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
        await InitializeDatabaseAsync(bitcoinPriceDbContext, cancellationToken);
    }

    private async Task InitializeDatabaseAsync(BitcoinPriceDbContext bitcoinPriceDbContext, CancellationToken cancellationToken)
    {
        using var activity = _activitySource.StartActivity("Initializing bitcoin price database", ActivityKind.Client);

        var sw = Stopwatch.StartNew();

        var strategy = bitcoinPriceDbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(bitcoinPriceDbContext.Database.MigrateAsync, cancellationToken);

        // todo: init with historical data from coindesk api

        logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms",
            sw.ElapsedMilliseconds);
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