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

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services
    .AddDomain()
    .AddApplication()
    .AddExternalApis()
    .AddRepositories()
    .AddProblemDetails()
    .AddSwaggerGen(options => { options.SwaggerDoc("v1", new() { Title = "Hodler.ApiService", Version = "v1" }); })
    .AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });

builder.Services
    .AddMvcCore()
    .AddApiExplorer();

AddAuthentication(builder);

builder.AddNpgsqlDbContext<PortfolioDbContext>("hodler-db");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // To serve the Swagger UI at the app's root
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
        options => options.UseNpgsql("identity-db")
    );

    webApplicationBuilder.Services
        .AddIdentityCore<User>()
        .AddEntityFrameworkStores<UserDbContext>()
        .AddApiEndpoints();
}