using Hodler.Contracts.PriceCatalogs;
using Hodler.Contracts.Shared;
using Hodler.Domain.PriceCatalogs.Models;
using Hodler.Domain.Shared.Models;
using Mapster;
using FiatCurrency = Hodler.Contracts.Shared.FiatCurrency;

namespace Hodler.ApiService.Mappings;

public class FiatAmountCatalogRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<IFiatAmountCatalog, BitcoinPricePerCurrencyCatalogDto>()
            .MapWith(x => new BitcoinPricePerCurrencyCatalogDto()
            {
                Catalog = x
                    .Select(fiatAmount => fiatAmount.Adapt<FiatAmountDto>())
                    .ToList()
            });

        config
            .NewConfig<FiatAmount, FiatAmountDto>()
            .MapWith(x => new FiatAmountDto()
            {
                Amount = x.Amount,
                FiatCurrency = (FiatCurrency)x.FiatCurrency.Id
            });
    }
}