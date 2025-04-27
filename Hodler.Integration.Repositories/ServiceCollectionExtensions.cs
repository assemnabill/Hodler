using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Portfolios.Ports.Repositories;
using Hodler.Domain.Users.Ports;
using Hodler.Integration.Repositories.BitcoinPrices.Repositories;
using Hodler.Integration.Repositories.Portfolios.Repositories;
using Hodler.Integration.Repositories.Users.Repositories;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Integration.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPortfolioRepository, PortfolioRepository>();
        services.AddScoped<IBitcoinPriceRepository, BitcoinPriceRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        TypeAdapterConfig.GlobalSettings.Scan(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }
}