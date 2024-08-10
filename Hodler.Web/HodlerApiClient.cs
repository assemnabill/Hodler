namespace Hodler.Web;

public class HodlerApiClient(HttpClient httpClient)
{
    public async Task<TransactionsSummaryReport?> GetTransactionsSummaryReportAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<TransactionsSummaryReport>("/transactions-summary-report", cancellationToken);
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record TransactionsSummaryReport(
    double NetInvestedFiat,
    double TotalBtcInvestment,
    double CurrentBtcPrice,
    double CurrentValue,
    double TotalProfitFiat,
    double TotalProfitPercentage,
    double AverageBtcPrice,
    double TaxFreeTotalBtcInvestment,
    double TaxFreeProfit);


