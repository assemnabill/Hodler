namespace Hodler.Domain.Shared.Models;

public record BitcoinAmount
{
    public decimal Amount { get; }

    public BitcoinAmount(decimal Amount)
    {
        this.Amount = Math.Abs(Amount);
    }

    public static implicit operator decimal(BitcoinAmount bitcoinAmount) => bitcoinAmount.Amount;

    public static implicit operator BitcoinAmount(decimal amount) => new(amount);
};