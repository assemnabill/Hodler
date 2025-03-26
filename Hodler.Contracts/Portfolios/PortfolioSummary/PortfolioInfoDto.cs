namespace Hodler.Contracts.Portfolios.PortfolioSummary;

// TODO ADD portfolioValue, fiatReturnOnInvestmentPercentage
public record PortfolioInfoDto(
    Guid Id,
    IReadOnlyCollection<TransactionInfoDto> Transactions
);
// TODO ADD portfolioValue, fiatReturnOnInvestmentPercentage