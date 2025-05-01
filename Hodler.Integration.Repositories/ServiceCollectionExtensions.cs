using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Portfolios.Ports.Repositories;
using Hodler.Domain.Users.Ports;
using Hodler.Integration.Repositories.BitcoinPrices.Repositories;
using Hodler.Integration.Repositories.Migrations;
using Hodler.Integration.Repositories.Portfolios.Repositories;
using Hodler.Integration.Repositories.Users.Repositories;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Integration.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(ServiceCollectionExtensions).Assembly);

        services.AddScoped<IPortfolioRepository, PortfolioRepository>();
        services.AddScoped<IBitcoinPriceRepository, BitcoinPriceRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Database Initialization
        services.AddSingleton<HodlerDbInitializer>();
        services.AddHostedService(sp => sp.GetRequiredService<HodlerDbInitializer>());


        return services;
    }
}