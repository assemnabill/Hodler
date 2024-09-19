using Hodler.Domain.Portfolio.Models;

namespace Hodler.Domain.Portfolio.Services;

public interface ITransactionParser
{
    ITransactions ParseTransactions(IEnumerable<string[]> lines);
    Transaction? ParseTransaction(string[] line, PortfolioId portfolioId);
}