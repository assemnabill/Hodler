using Hodler.Integration.Repositories;
using Hodler.Integration.Repositories.BitcoinPrices.Context;
using Hodler.Integration.Repositories.Portfolios.Context;
using Hodler.Integration.Repositories.Users.Context;
using Hodler.Integration.Repositories.Users.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Hodler.ApiService;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddAuthentication(
        this WebApplicationBuilder builder,
        IConfiguration configuration
    )
    {
        //builder.Services
        //    .AddAuthentication(options =>
        //    {
        //        options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
        //        options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
        //    })
        //    .AddCookie(IdentityConstants.BearerScheme, options =>
        //    {
        //        options.Cookie.SameSite = SameSiteMode.None;
        //        options.Cookie.HttpOnly = true;
        //        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        //        options.SlidingExpiration = true;
        //    });
        //JWT
        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;
        })
                        .AddEntityFrameworkStores<UserDbContext>()
                        .AddDefaultTokenProviders(); ;
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

    public static WebApplicationBuilder AddRepositories(
        this WebApplicationBuilder builder
    )
    {
        builder.AddNpgsqlDbContext<PortfolioDbContext>(ServiceConstants.DatabaseName, null, ConfigureDbContextOptions);
        builder.AddNpgsqlDbContext<BitcoinPriceDbContext>(ServiceConstants.DatabaseName, null, ConfigureDbContextOptions);

        builder.Services.AddRepositories();

        return builder;

        void ConfigureDbContextOptions(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder
                .UseNpgsql(ob => ob.MigrationsAssembly(typeof(BitcoinPriceDbContext).Assembly.GetName().Name));
    }

    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc
                (
                    ServiceConstants.SwaggerDocumentName,
                    new OpenApiInfo
                    {
                        Title = ServiceConstants.Title,
                        Description = null,
                        Version = ServiceConstants.ApiVersion,
                        TermsOfService = null,
                        Contact = null,
                        License = null,
                        Extensions = null
                    }
                );


                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });


                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                       {
                           {
                               new OpenApiSecurityScheme
                               {
                                   Reference = new OpenApiReference
                                   {
                                       Type = ReferenceType.SecurityScheme,
                                       Id = "Bearer"
                                   },
                                   Scheme = "oauth2",
                                   Name = "Bearer",
                                   In = ParameterLocation.Header,
                               },
                               new List<string>()
                           }
                       });

            });

        return builder;
    }
}