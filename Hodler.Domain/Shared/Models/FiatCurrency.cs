using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Domain.Shared.Models;

public class FiatCurrency : TypeSafeEnum<FiatCurrency>
{
    public string Ticker { get; }
    public string Symbol { get; }

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

    public FiatCurrency(int id, string ticker, string symbol) : base(id)
    {
        Ticker = ticker;
        Symbol = symbol;
    }
}