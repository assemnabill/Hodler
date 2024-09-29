namespace Hodler.Contracts.Portfolio.PortfolioSummary;

public record PortfolioSummaryDto(
    decimal NetInvestedFiat,
    decimal TotalBtcInvestment,
    decimal CurrentBtcPrice,
    decimal CurrentValue,
    decimal ProfitLossInFiat,
    double ProfitLossInFiatPercentage,
    decimal AverageBtcPrice,
    decimal TaxFreeBtcInvestment,
    double TaxFreeProfitPercentage);