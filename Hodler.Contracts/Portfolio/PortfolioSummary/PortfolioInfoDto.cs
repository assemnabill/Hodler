namespace Hodler.Contracts.Portfolio.PortfolioSummary;

public record PortfolioInfoDto(
    Guid Id,
    IReadOnlyCollection<TransactionInfoDto> Transactions
);