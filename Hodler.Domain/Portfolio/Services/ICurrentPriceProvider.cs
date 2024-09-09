namespace Hodler.Domain.Portfolio.Services;

public interface ICurrentPriceProvider
{
    Task<double> GetCurrentPriceAsync(CancellationToken cancellationToken);
}