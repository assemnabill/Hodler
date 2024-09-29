namespace Hodler.Domain.Shared.Models;

public record FiatAmount
{
    public decimal Amount { get; }
    public FiatCurrency FiatCurrency { get; }

    public FiatAmount(decimal Amount, FiatCurrency FiatCurrency)
    {
        this.Amount = Math.Abs(Amount);
        this.FiatCurrency = FiatCurrency;
    }

    public static implicit operator decimal(FiatAmount fiatAmount) => fiatAmount.Amount;
};