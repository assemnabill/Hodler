namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public record TransactionHash(string Value)
{
    public static implicit operator string(TransactionHash transactionHash) => transactionHash.Value;
    public static implicit operator TransactionHash(string value) => new(value);
}