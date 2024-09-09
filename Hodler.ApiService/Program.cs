using Hodler.Application;
using Hodler.Domain;
using Hodler.Integration.Repositories;
using Hodler.Integrations.ExternalApis;
using Hodler.ServiceDefaults;
using Mapster;
using ServiceCollectionExtensions = Hodler.Application.ServiceCollectionExtensions;

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

builder.Services.AddMvcCore()
    .AddApiExplorer();

// Tell Mapster to scan this assambly searching for the Mapster.IRegister
// classes and execute them
TypeAdapterConfig.GlobalSettings.Scan(typeof(ServiceCollectionExtensions).Assembly);

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
    });}

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

app.Run();