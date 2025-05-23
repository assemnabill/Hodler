using Hodler.Domain.Shared.Aggregate;

namespace Hodler.Domain.Shared.Models;

public class FiatCurrency : TypeSafeEnum<FiatCurrency>
{
    public static readonly FiatCurrency Euro = new(1, "EUR", "€");
    public static readonly FiatCurrency UsDollar = new(2, "USD", "$");
    public static readonly FiatCurrency SwissFranc = new(3, "CHF", "CHF");
    public static readonly FiatCurrency BritishPound = new(4, "GBP", "£");
    public static readonly FiatCurrency TurkishLira = new(5, "TRY", "₺");
    public static readonly FiatCurrency PolishZloty = new(6, "PLN", "zł");
    public static readonly FiatCurrency HungarianForint = new(7, "HUF", "Ft");
    public static readonly FiatCurrency CzechKoruna = new(8, "CZK", "Kč");
    public static readonly FiatCurrency SwedishKrona = new(15, "SEK", "kr");
    public static readonly FiatCurrency DanishKrone = new(17, "DKK", "kr");
    public string Ticker { get; }
    public string Symbol { get; }

    public FiatCurrency(int id, string ticker, string symbol) : base(id)
    {
        Ticker = ticker;
        Symbol = symbol;
    }


    public static FiatCurrency GetByTicker(string ticker)
    {
        return ticker.ToUpper() switch
        {
            "EUR" => Euro,
            "USD" => UsDollar,
            "CHF" => SwissFranc,
            "GBP" => BritishPound,
            "TRY" => TurkishLira,
            "PLN" => PolishZloty,
            "HUF" => HungarianForint,
            "CZK" => CzechKoruna,
            "SEK" => SwedishKrona,
            "DKK" => DanishKrone,
            _ => throw new ArgumentOutOfRangeException(nameof(ticker), ticker, null)
        };
    }

    public static FiatCurrency GetById(int id)
    {
        return id switch
        {
            1 => Euro,
            2 => UsDollar,
            3 => SwissFranc,
            4 => BritishPound,
            5 => TurkishLira,
            6 => PolishZloty,
            7 => HungarianForint,
            8 => CzechKoruna,
            15 => SwedishKrona,
            17 => DanishKrone,
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }
}