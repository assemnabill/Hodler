namespace Hodler.Contracts.Portfolios.PortfolioSummary;

public record PortfolioInfoDto(
    Guid Id,
    IReadOnlyCollection<TransactionInfoDto> Transactions
);