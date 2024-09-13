using Hodler.Application;
using Hodler.Domain;
using Hodler.Integration.Repositories;
using Hodler.Integration.Repositories.Portfolio.Context;
using Hodler.Integration.ExternalApis;
using Hodler.Integration.Repositories.User.Context;
using Hodler.Integration.Repositories.User.Entities;
using Hodler.ServiceDefaults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

const string connectionString = "hodler-db";
const string apiVersion = "v1";

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services
    .AddDomain()
    .AddApplication()
    .AddExternalApis()
    .AddRepositories()
    .AddProblemDetails()
    .AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });

builder.Services
    .AddMvcCore()
    .AddApiExplorer();

AddAuthentication(builder);
AddDbContexts(builder);
AddSwagger(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            $"Hodler API {apiVersion}");
        c.RoutePrefix = "hodler"; // To serve the Swagger UI at the app's root
    });
}

// TODO: Validation        

// app.MapGet("/transactions-summary-report", async () =>
// {
//     var service = app.Services.GetRequiredService<ITransactionsQueryService>();
//     var summaryReport = await service.GetTransactionsSummaryReportAsync(default);
//     return summaryReport;
// });
//
// app.MapGet("/transactions", async () =>
// {
//     var service = app.Services.GetRequiredService<ITransactionsQueryService>();
//     var transactions = await service.GetTransactionsAsync(default);
//     return transactions;
// });
//
// app.MapGet("/transactions/sync/bitpanda", async () =>
// {
//     var service = app.Services.GetRequiredService<ITransactionsQueryService>();
//     var transactions = await service.GetTransactionsAsync(default);
//     return transactions;
// });
app.MapDefaultEndpoints();
app.MapControllers();
app.MapIdentityApi<User>();
app.Run();


void AddAuthentication(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services
        .AddAuthentication(IdentityConstants.ApplicationScheme)
        .AddIdentityCookies();

    webApplicationBuilder.Services.AddAuthorizationBuilder();

    webApplicationBuilder.Services.AddDbContext<UserDbContext>(
        options => options.UseNpgsql(connectionString)
    );

    webApplicationBuilder.Services
        .AddIdentityCore<User>()
        .AddEntityFrameworkStores<UserDbContext>()
        .AddApiEndpoints();
}

void AddDbContexts(WebApplicationBuilder builder1)
{
    builder1.AddNpgsqlDbContext<PortfolioDbContext>(connectionString);
}

void AddSwagger(WebApplicationBuilder webApplicationBuilder1)
{
    // Swagger/OpenAPI services
    webApplicationBuilder1.Services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                apiVersion, new()
                {
                    Title = "Hodler.Api",
                    Version = apiVersion
                });
        });
}