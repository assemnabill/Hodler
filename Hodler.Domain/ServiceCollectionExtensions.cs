using Corz.DomainDriven.Abstractions.DomainEvents;
using Hodler.Domain.BitcoinPrices.Services;
using Hodler.Domain.Shared.EmailService;
using Hodler.Domain.Shared.Services;
using Hodler.Domain.Token.Models;
using Hodler.Domain.Token.Services;
using Hodler.Domain.Users.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Hodler.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {

        services
            .AddTransient<IBitcoinPriceSyncService, BitcoinPriceSyncService>()
            .AddTransient<IPriceCatalogService, PriceCatalogService>()
            .AddTransient<IUserSettingsQueryService, UserSettingsQueryService>()
            .AddTransient<IUserSettingsService, UserSettingsService>();

        services
            .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();

        services
            .AddSingleton<ISystemClock, SystemClock>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }

    public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services,
                                                               IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(jwtSettings.ClockSkewMinutes)
            };
        });
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        return services;
    }

    public static IServiceCollection AddMailService(this IServiceCollection services,
                                                    IConfiguration configuration)
    {
        services.AddTransient<IEmailService, MailKitEmailService>();
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        return services;
    }

}