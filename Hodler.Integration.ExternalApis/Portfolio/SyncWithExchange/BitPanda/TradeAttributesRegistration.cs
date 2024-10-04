using Bitpanda.RestClient;
using Hodler.Domain.CryptoExchange.Models;
using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Shared.Models;
using Mapster;
using TransactionType = Hodler.Domain.Portfolio.Models.TransactionType;

namespace Hodler.Integration.ExternalApis.Portfolio.SyncWithExchange.BitPanda;

public class TradeAttributesRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<TradeAttributes, Transaction>()
            .MapWith(src => new Transaction(
                new PortfolioId(Guid.NewGuid()),
                src.Type == TradeType.Buy ? TransactionType.Buy : TransactionType.Sell,
                new FiatAmount(src.Amount_fiat, FiatCurrency.GetById(int.Parse(src.Fiat_id))),
                new BitcoinAmount(src.Amount_cryptocoin),
                new FiatAmount(src.Price, FiatCurrency.GetById(int.Parse(src.Fiat_id))),
                DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(src.Time.Unix)).ToUniversalTime(),
                CryptoExchangeNames.BitPanda
            ));

        config
            .NewConfig<TradeAttributes, TransactionInfo>()
            .MapWith(src => new TransactionInfo(
                src.Type == TradeType.Buy ? TransactionType.Buy : TransactionType.Sell,
                new FiatAmount(src.Amount_fiat, FiatCurrency.GetById(int.Parse(src.Fiat_id))),
                new BitcoinAmount(src.Amount_cryptocoin),
                new FiatAmount(src.Price, FiatCurrency.GetById(int.Parse(src.Fiat_id))),
                src.Time.Date_iso8601.ToUniversalTime(),
                CryptoExchangeNames.BitPanda
            ));
    }
}