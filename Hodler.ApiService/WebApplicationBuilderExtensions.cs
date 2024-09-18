using Hodler.Integration.Repositories.Portfolio.Context;
using Hodler.Integration.Repositories.User.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hodler.ApiService;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddAuthentication(
        this WebApplicationBuilder builder,
        IConfiguration configuration)
    {
        builder.Services
            .AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();

        builder.Services.AddAuthorizationBuilder();

        builder.Services.AddDbContext<UserDbContext>(
            options =>
            {
                var connectionString = configuration.GetConnectionString(ServiceConstants.DatabaseName);

                options.UseNpgsql(connectionString);
            });

        builder.Services
            .AddIdentityCore<Integration.Repositories.User.Entities.User>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddApiEndpoints();

        return builder;
    }

    public static WebApplicationBuilder AddDbContexts(
        this WebApplicationBuilder builder
    )
    {
        builder.AddNpgsqlDbContext<PortfolioDbContext>(ServiceConstants.DatabaseName);

        return builder;
    }

    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    ServiceConstants.ApiVersion,
                    new()
                    {
                        Title = "Hodler.Api",
                        Version = ServiceConstants.ApiVersion
                    });
            });

        return builder;
    }
}