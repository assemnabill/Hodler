using Hodler.ApiService;
using Hodler.Domain;
using Hodler.Domain.Transactions.Services;
using Hodler.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services
    .AddDomain()
    .AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet("/transactions-summary-report", () =>
{
    var service = app.Services.GetRequiredService<ITransactionsQueryService>();
    var summaryReport = service.GetTransactionsSummaryReport();
    return summaryReport;
});

app.MapDefaultEndpoints();

app.Run();

namespace Hodler.ApiService
{
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
