using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.Shared.Models;
using Mapster;
using BitcoinPrice = Hodler.Integration.Repositories.BitcoinPrices.Entities.BitcoinPrice;

namespace Hodler.Integration.Repositories.BitcoinPrices.Mappings;

public class BitcoinPriceMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<BitcoinPrice, IBitcoinPrice>()
            .ConstructUsing(x => new Domain.BitcoinPrices.Models.BitcoinPrice(
                new DateOnly(x.Date.Year, x.Date.Month, x.Date.Day),
                FiatCurrency.GetById(x.Currency),
                new FiatAmount(x.Close, FiatCurrency.GetById(x.Currency)),
                x.Open == null ? null : new FiatAmount(x.Open.Value, FiatCurrency.GetById(x.Currency)),
                x.High == null ? null : new FiatAmount(x.High.Value, FiatCurrency.GetById(x.Currency)),
                x.Low == null ? null : new FiatAmount(x.Low.Value, FiatCurrency.GetById(x.Currency)),
                x.Volume == null ? null : new FiatAmount(x.Volume.Value, FiatCurrency.GetById(x.Currency))
            ));

        config
            .NewConfig<IBitcoinPrice, BitcoinPrice>()
            .MapWith(x => new BitcoinPrice
            {
                Date = x.Date,
                Currency = x.Currency.Id,
                Close = x.Close.Amount,
                Open = x.Open != null ? x.Open.Amount : null,
                High = x.High != null ? x.High.Amount : null,
                Low = x.Low != null ? x.Low.Amount : null,
                Volume = x.Volume != null ? x.Volume.Amount : null,
            });
    }
}