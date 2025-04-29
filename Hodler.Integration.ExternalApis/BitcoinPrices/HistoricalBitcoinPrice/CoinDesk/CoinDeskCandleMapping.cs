using Corz.Extensions.DateTime;
using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.Shared.Models;
using Mapster;

namespace Hodler.Integration.ExternalApis.BitcoinPrices.HistoricalBitcoinPrice.CoinDesk;

public class CoinDeskCandleMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .ForType<CoinDeskCandle, BitcoinPrice>()
            .ConstructUsing(candle => new BitcoinPrice(
                DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(candle.Timestamp).DateTime),
                FiatCurrency.GetByTicker(candle.Ticker),
                new FiatAmount((decimal)candle.Close, FiatCurrency.GetByTicker(candle.Ticker)),
                new FiatAmount((decimal)candle.Open, FiatCurrency.GetByTicker(candle.Ticker)),
                new FiatAmount((decimal)candle.High, FiatCurrency.GetByTicker(candle.Ticker)),
                new FiatAmount((decimal)candle.Low, FiatCurrency.GetByTicker(candle.Ticker)),
                new FiatAmount((decimal)candle.Volume, FiatCurrency.GetByTicker(candle.Ticker))
            ));

        config
            .ForType<long, DateOnly>()
            .ConstructUsing(x => DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(x).DateTime));

        config
            .ForType<DateOnly, long>()
            .ConstructUsing(x => x.ToDateTimeOffset().ToUnixTimeSeconds());
    }
}