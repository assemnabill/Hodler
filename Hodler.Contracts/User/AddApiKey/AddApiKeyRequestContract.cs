namespace Hodler.Contracts.User.AddApiKey;

public class AddApiKeyRequestContract
{
    public Guid UserId { get; set; }
    public string ApiKeyValue { get; set; }
    public string ApiKeyName { get; set; }
    public string? Secret { get; set; }
}