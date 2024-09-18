namespace Hodler.Contracts.Portfolio.PortfolioSummary;

public record PortfolioSummaryDto(
    double NetInvestedFiat,
    double TotalBtcInvestment,
    double CurrentBtcPrice,
    double CurrentValue,
    double ProfitLossInFiat,
    double ProfitLossInFiatPercentage,
    double AverageBtcPrice,
    double TaxFreeBtcInvestment,
    double TaxFreeProfitPercentage);