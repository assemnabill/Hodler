using Hodler.Contracts.Portfolio;

namespace Hodler.Web;

public class HodlerApiClient(HttpClient httpClient)
{
    public async Task<TransactionsSummaryReport?> GetTransactionsSummaryReportAsync(int maxItems = 10,
        CancellationToken cancellationToken = default)
    {
        return await httpClient
            .GetFromJsonAsync<TransactionsSummaryReport>(
                "/transactions-summary-report",
                cancellationToken);
    }
    
    public async Task<TransactionViewModel[]?> GetTransactionsAsync(int maxItems = 10,
        CancellationToken cancellationToken = default)
    {
        return await httpClient
            .GetFromJsonAsync<TransactionViewModel[]>(
                "/transactions",
                cancellationToken);
    }
}

public record TransactionsSummaryReport(
    double NetInvestedFiat,
    double TotalBtcInvestment,
    double CurrentBtcPrice,
    double CurrentValue,
    double ProfitLossInFiat,
    double ProfitLossInFiatPercentage,
    double AverageBtcPrice,
    double TaxFreeBtcInvestment,
    double TaxFreeProfitPercentage);
    
public record TransactionViewModel(
    TransactionType Type,
    double FiatAmount,
    double BtcAmount,
    double MarketPrice,
    DateTimeOffset Timestamp);