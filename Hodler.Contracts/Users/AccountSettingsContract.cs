namespace Hodler.Contracts.Users;

public class AccountSettingsContract
{
    public FiatCurrencyContract DisplayCurrency { get; set; }
    public string Theme { get; set; }
}