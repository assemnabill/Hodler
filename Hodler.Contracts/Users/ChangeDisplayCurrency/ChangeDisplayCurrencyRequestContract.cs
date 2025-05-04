using Hodler.Contracts.Shared;

namespace Hodler.Contracts.Users.ChangeDisplayCurrency;

public class ChangeDisplayCurrencyRequestContract
{
    public FiatCurrency NewDisplayCurrency { get; set; }
}