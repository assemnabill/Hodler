namespace Hodler.Contracts.Shared;

public class FiatAmountDto
{
    public decimal Amount { get; set; }
    public FiatCurrency FiatCurrency { get; set; }
}