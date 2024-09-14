using Hodler.ApiService;
using Hodler.Application;
using Hodler.Domain;
using Hodler.Integration.Repositories;
using Hodler.Integration.ExternalApis;
using Hodler.Integration.Repositories.User.Entities;
using Hodler.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Hodler Service Layers.
builder.Services
    .AddDomain()
    .AddApplication()
    .AddExternalApis()
    .AddRepositories();

// Hodler Service Core
builder.Services
    .AddProblemDetails()
    .AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); })
    .AddMvcCore()
    .AddApiExplorer();

// Hodler Service Infrastructure
builder.AddRedisDistributedCache("cache");
builder.AddAuthentication();
builder.AddDbContexts();
builder.AddSwagger();

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
            $"Hodler API {ServiceConstants.ApiVersion}");
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
