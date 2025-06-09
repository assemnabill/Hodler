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
    public static implicit operator FiatAmount(decimal amount) => new(amount, FiatCurrency.UsDollar);

    public static FiatAmount operator +(FiatAmount left, FiatAmount right)
    {
        if (left.FiatCurrency != right.FiatCurrency)
            throw new InvalidOperationException("Cannot add amounts in different currencies.");

        return new FiatAmount(left.Amount + right.Amount, left.FiatCurrency);
    }

    public static FiatAmount operator -(FiatAmount left, FiatAmount right)
    {
        if (left.FiatCurrency != right.FiatCurrency)
            throw new InvalidOperationException("Cannot subtract amounts in different currencies.");

        return new FiatAmount(left.Amount - right.Amount, left.FiatCurrency);
    }

    public static FiatAmount operator *(FiatAmount left, FiatAmount right)
    {
        if (left.FiatCurrency != right.FiatCurrency)
            throw new InvalidOperationException("Cannot multiply amounts in different currencies.");

        return new FiatAmount(left.Amount * right.Amount, left.FiatCurrency);
    }

    public static FiatAmount operator /(FiatAmount left, FiatAmount right)
    {
        if (left.FiatCurrency != right.FiatCurrency)
            throw new InvalidOperationException("Cannot multiply amounts in different currencies.");

        if (right == 0)
            throw new DivideByZeroException("Cannot divide by zero.");

        return new FiatAmount(left.Amount / right.Amount, left.FiatCurrency);
    }


    public override string ToString() => $"{Amount} {FiatCurrency.Symbol}";

    public FiatAmount ConvertTo(FiatCurrency otherCurrency)
    {
        // todo: implement conversion logic
        // var conversionRate = FiatCurrency.GetConversionRate(otherCurrency);
        var conversionRate = 1;

        return new FiatAmount(Amount * conversionRate, otherCurrency);
    }

    public static FiatAmount Zero(FiatCurrency fiatCurrency) => new(0, fiatCurrency);
};