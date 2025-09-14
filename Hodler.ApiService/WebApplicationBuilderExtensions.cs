using Hodler.Domain.Token.Models;
using Hodler.Integration.Repositories;
using Hodler.Integration.Repositories.BitcoinPrices.Context;
using Hodler.Integration.Repositories.Portfolios.Context;
using Hodler.Integration.Repositories.Users.Context;
using Hodler.Integration.Repositories.Users.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;
        })
                        .AddEntityFrameworkStores<UserDbContext>()
                        .AddDefaultTokenProviders()
                        .AddApiEndpoints();

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

        /*
        builder.Services
            .AddIdentityCore<User>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders()
            .AddApiEndpoints();
            */
        builder.Services.AddAuthentication(options =>
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
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["jwt"];
                    return Task.CompletedTask;
                }
            };
        });
        
        builder.Services.AddAntiforgery(options =>
        {
            // Cookie configuration
            options.Cookie.Name = "XSRF-TOKEN";
            options.Cookie.HttpOnly = false; // Allow JavaScript access
            options.Cookie.SameSite = SameSiteMode.Strict; // CSRF protection
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
            options.Cookie.IsEssential = true; // Required for GDPR compliance
    
            // Header configuration
            options.HeaderName = "X-XSRF-TOKEN";
    
            // Additional security
            options.SuppressXFrameOptionsHeader = false; // Keep X-Frame-Options
        });
        
        builder.Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
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