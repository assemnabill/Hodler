namespace Hodler.Contracts.Users.AddApiKey;

public class AddApiKeyRequestContract
{
    public string ApiKeyValue { get; set; }
    public string ApiKeyName { get; set; }
    public string? Secret { get; set; }
}