using System.Text;
using Hodler.Integration.Repositories.Portfolio.Context;
using Hodler.Integration.Repositories.User.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hodler.ApiService;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddJwtBasedAuthentication(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        builder.Services
            .AddIdentity<Integration.Repositories.User.Entities.User, IdentityRole>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthorization();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        builder.Services.AddDbContext<UserDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString(ServiceConstants.DatabaseName);
            options.UseNpgsql(connectionString);
        });

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