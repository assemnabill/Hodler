namespace Hodler.Domain.Transactions.Services;

public interface ICurrentPriceProvider
{
    Task<double> GetCurrentPriceAsync(CancellationToken cancellationToken);
}