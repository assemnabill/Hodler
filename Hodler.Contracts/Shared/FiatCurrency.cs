using System.ComponentModel;

namespace Hodler.Contracts.Shared;

public enum FiatCurrency
{
    [Description("EUR")] Euro = 1,
    [Description("USD")] UsDollar = 2,
    [Description("CHF")] SwissFranc = 3,
    [Description("GBP")] BritishPound = 4,
    [Description("TRY")] TurkishLira = 5,
    [Description("PLN")] PolishZloty = 6,
    [Description("HUF")] HungarianForint = 7,
    [Description("CZK")] CzechKoruna = 8,
    [Description("SEK")] SwedishKrona = 15,
    [Description("DKK")] DanishKrone = 17
}