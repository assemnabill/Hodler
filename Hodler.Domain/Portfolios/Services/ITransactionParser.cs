using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Portfolios.Models.Transactions;

namespace Hodler.Domain.Portfolios.Services;

public interface ITransactionParser
{
    IManualTransactions ParseTransactions(IEnumerable<string[]> lines);
    Transaction? ParseTransaction(string[] line, PortfolioId portfolioId);
}