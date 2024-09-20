namespace Hodler.Domain.User.Models;

public class ApiKey
{
    public ApiKeyId ApiKeyId { get; }
    public UserId UserId { get; }
    public ApiName ApiName { get; }
    public string Value { get; }

    public ApiKey(ApiKeyId apiKeyId, ApiName apiName, string value, UserId userId)
    {
        ArgumentNullException.ThrowIfNull(apiKeyId);
        ArgumentNullException.ThrowIfNull(apiName);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentNullException.ThrowIfNull(userId);

        ApiKeyId = apiKeyId;
        ApiName = apiName;
        Value = value;
        UserId = userId;
    }

    public override string ToString() => Value;
}