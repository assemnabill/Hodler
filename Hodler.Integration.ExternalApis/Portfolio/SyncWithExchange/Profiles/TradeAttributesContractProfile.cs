using Bitpanda.RestClient;
using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Shared.Models;
using Mapster;

namespace Hodler.Integration.ExternalApis.Portfolio.SyncWithExchange.Profiles;

public class TradeAttributesContractProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<TradeAttributes, Transaction>()
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.BtcAmount, src => src.Amount_cryptocoin)
            .Map(dest => dest.FiatAmount, src => src.Amount_fiat)
            .Map(dest => dest.Timestamp, src => DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(src.Time.Unix)))
            .Map(dest => dest.MarketPrice, src => src.Price)
            .Map(dest => dest.CryptoExchange, _ => CryptoExchange.BitPanda)
            .Map(dest => dest.FiatCurrency,
                src => FiatCurrency.AsEnumerable().FirstOrDefault(x => x.Id.ToString() == src.Fiat_id))
            ;
    }
}