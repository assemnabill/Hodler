namespace Hodler.Domain.Shared.Models;

public record BitcoinAmount
{
    public decimal Amount { get; }
    public decimal AmountInSatoshis => Amount * 100_000_000;
    public static BitcoinAmount Zero { get; } = new(0.0m);

    public BitcoinAmount(decimal amountInBtc)
    {
        Amount = System.Math.Abs(amountInBtc);
    }

    public static implicit operator decimal(BitcoinAmount bitcoinAmount) => bitcoinAmount.Amount;

    public static implicit operator BitcoinAmount(decimal amount) => new(amount);

    public override string ToString() => $"{Amount} {CryptoCurrency.Bitcoin.Symbol}";

    public static BitcoinAmount FromSatoshis(decimal amountInSatoshis)
    {
        return new BitcoinAmount(amountInSatoshis / 100_000_000);
    }
};