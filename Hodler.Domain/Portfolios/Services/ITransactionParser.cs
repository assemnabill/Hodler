using Hodler.Domain.Portfolios.Models;

namespace Hodler.Domain.Portfolios.Services;

public interface ITransactionParser
{
    ITransactions ParseTransactions(IEnumerable<string[]> lines);
    Transaction? ParseTransaction(string[] line, PortfolioId portfolioId);
}