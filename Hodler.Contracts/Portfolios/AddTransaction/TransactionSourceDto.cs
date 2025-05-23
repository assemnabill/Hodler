namespace Hodler.Contracts.Portfolios.AddTransaction;

public class TransactionSourceDto
{
    public int Type { get; set; }
    public string Identifier { get; set; }
    public string? Name { get; set; }
}