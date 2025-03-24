using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalogs.Models;

public interface IFiatAmountCatalog : IReadOnlyCollection<FiatAmount>
{
    static IReadOnlyCollection<FiatCurrency> SupportedFiatCurrencies
    {
        get
        {
            IReadOnlyCollection<FiatCurrency> supportedFiatCurrencies =
            [
                FiatCurrency.Euro,
                FiatCurrency.UsDollar,
                FiatCurrency.SwissFranc,
                FiatCurrency.BritishPound,
                FiatCurrency.TurkishLira,
                FiatCurrency.PolishZloty,
                FiatCurrency.HungarianForint,
                FiatCurrency.CzechKoruna,
                FiatCurrency.SwedishKrona,
                FiatCurrency.DanishKrone
            ];
            
            return supportedFiatCurrencies;
        }
    }

    FiatAmount GetPrice(FiatCurrency currency);
}