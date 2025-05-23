namespace Hodler.Domain.Shared.Models;

public record FiatAmount
{
    public decimal Amount { get; }
    public FiatCurrency FiatCurrency { get; }

    public static FiatAmount ZeroUsDollars => new(0, FiatCurrency.UsDollar);

    public FiatAmount(decimal Amount, FiatCurrency FiatCurrency)
    {
        this.Amount = System.Math.Abs(Amount);
        this.FiatCurrency = FiatCurrency;
    }

    public static implicit operator decimal(FiatAmount fiatAmount) => fiatAmount.Amount;

    public FiatAmount ConvertTo(FiatCurrency otherCurrency)
    {
        // todo: implement conversion logic
        // var conversionRate = FiatCurrency.GetConversionRate(otherCurrency);
        var conversionRate = 1;

        return new FiatAmount(Amount * conversionRate, otherCurrency);
    }

    public static FiatAmount Zero(FiatCurrency fiatCurrency) => new(0, fiatCurrency);
};