using Bitpanda.RestClient;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Shared.Models;
using Mapster;
using TransactionType = Hodler.Domain.Portfolios.Models.TransactionType;

namespace Hodler.Integration.ExternalApis.Portfolios.SyncWithExchange.BitPanda;

public class TradeAttributesRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<TradeAttributes, Transaction>()
            .MapWith(src => new Transaction(
                new PortfolioId(Guid.NewGuid()),
                new TransactionId(Guid.NewGuid()),
                src.Type == TradeType.Buy ? TransactionType.Buy : TransactionType.Sell,
                new FiatAmount(src.Amount_fiat, FiatCurrency.GetById(int.Parse(src.Fiat_id))),
                new BitcoinAmount(src.Amount_cryptocoin),
                DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(src.Time.Unix)).ToUniversalTime(),
                new FiatAmount(src.Price, FiatCurrency.GetById(int.Parse(src.Fiat_id))),
                CryptoExchangeName.BitPanda
            ));

        config
            .NewConfig<TradeAttributes, TransactionInfo>()
            .MapWith(src => new TransactionInfo(
                new TransactionId(Guid.NewGuid()),
                src.Type == TradeType.Buy ? TransactionType.Buy : TransactionType.Sell,
                new FiatAmount(src.Amount_fiat, FiatCurrency.GetById(int.Parse(src.Fiat_id))),
                new BitcoinAmount(src.Amount_cryptocoin),
                new FiatAmount(src.Price, FiatCurrency.GetById(int.Parse(src.Fiat_id))),
                src.Time.Date_iso8601.ToUniversalTime(),
                CryptoExchangeName.BitPanda
            ));
    }
}