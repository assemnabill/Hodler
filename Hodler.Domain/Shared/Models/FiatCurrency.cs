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
    public static readonly FiatCurrency JapaneseYen = new(5, "JPY", "¥");
    public static readonly FiatCurrency ChineseYuan = new(6, "CNY", "¥");
    public static readonly FiatCurrency RussianRuble = new(7, "RUB", "₽");
    public static readonly FiatCurrency IndianRupee = new(8, "INR", "₹");
    public static readonly FiatCurrency BrazilianReal = new(9, "BRL", "R$");
    public static readonly FiatCurrency SouthKoreanWon = new(10, "KRW", "₩");
    public static readonly FiatCurrency CanadianDollar = new(11, "CAD", "$");
    public static readonly FiatCurrency AustralianDollar = new(12, "AUD", "$");
    public static readonly FiatCurrency NewZealandDollar = new(13, "NZD", "$");
    public static readonly FiatCurrency SouthAfricanRand = new(14, "ZAR", "R");
    public static readonly FiatCurrency SwedishKrona = new(15, "SEK", "kr");
    public static readonly FiatCurrency NorwegianKrone = new(16, "NOK", "kr");
    public static readonly FiatCurrency DanishKrone = new(17, "DKK", "kr");
    public static readonly FiatCurrency SingaporeDollar = new(18, "SGD", "$");
    public static readonly FiatCurrency HongKongDollar = new(19, "HKD", "$");
    public static readonly FiatCurrency IndonesianRupiah = new(20, "IDR", "Rp");
    public static readonly FiatCurrency MalaysianRinggit = new(21, "MYR", "RM");
    

    public FiatCurrency(int id, string ticker, string symbol) : base(id)
    {
        Ticker = ticker;
        Symbol = symbol;
    }
}