namespace Hodler.Contracts.User.AddApiKey;

public class AddApiKeyRequestContract
{
    public Guid UserId { get; set; }
    public string ApiKey { get; set; }
    public string ApiName { get; set; }
}