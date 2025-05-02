using Hodler.Contracts.Shared;

namespace Hodler.Contracts.Users;

public class FiatCurrencyContract
{
    public FiatCurrency Id { get; set; }
    public string Ticker { get; set; }
    public string Symbol { get; set; }
}