namespace Hodler.Contracts.Portfolios.PortfolioSummary;

public record PortfolioTransactionsDto(
    Guid Id,
    IReadOnlyCollection<TransactionInfoDto> Transactions
);