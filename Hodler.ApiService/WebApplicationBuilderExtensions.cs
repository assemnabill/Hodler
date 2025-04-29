using Hodler.Integration.Repositories.BitcoinPrices.Context;
using Hodler.Integration.Repositories.Portfolios.Context;
using Hodler.Integration.Repositories.Users.Context;
using Hodler.Integration.Repositories.Users.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hodler.ApiService;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddAuthentication(
        this WebApplicationBuilder builder,
        IConfiguration configuration
    )
    {
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
                options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
            })
            .AddCookie(IdentityConstants.BearerScheme, options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
            });

        builder.Services.AddAuthorizationBuilder();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // Allow credentials (cookies)
            });
        });

        builder.Services.AddDbContext<UserDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString(ServiceConstants.DatabaseName);

            options.UseNpgsql(connectionString);
        });

        builder.Services
            .AddIdentityCore<User>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders()
            .AddApiEndpoints();

        return builder;
    }

    public static WebApplicationBuilder AddDbContexts(
        this WebApplicationBuilder builder
    )
    {
        builder.AddNpgsqlDbContext<BitcoinPriceDbContext>(ServiceConstants.DatabaseName);
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
                        Title = ServiceConstants.Title,
                        Version = ServiceConstants.ApiVersion
                    });
            });

        return builder;
    }
}