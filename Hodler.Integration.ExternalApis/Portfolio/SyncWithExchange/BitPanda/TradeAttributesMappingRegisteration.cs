using Bitpanda.RestClient;
using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Shared.Models;
using Mapster;
using TransactionType = Hodler.Domain.Portfolio.Models.TransactionType;

namespace Hodler.Integration.ExternalApis.Portfolio.SyncWithExchange.BitPanda;

public class TradeAttributesMappingRegisteration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<TradeAttributes, Transaction>()
            .MapWith(src => new Transaction(
                src.Type == TradeType.Buy ? TransactionType.Buy : TransactionType.Sell,
                new FiatAmount(
                    Convert.ToDouble(src.Amount_fiat),
                    FiatCurrency.AsEnumerable().FirstOrDefault(x => x.Id.ToString() == src.Fiat_id, FiatCurrency.Euro)),
                new BitcoinAmount(Convert.ToDouble(src.Amount_cryptocoin)),
                Convert.ToDouble(src.Price),
                DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(src.Time.Unix)),
                CryptoExchange.BitPanda
            ));
    }
}