using Hodler.Domain.CryptoExchange.Models;

namespace Hodler.Domain.User.Models;

public class ApiKey
{
    public ApiKeyId ApiKeyId { get; }
    public UserId UserId { get; }
    public ApiKeyName ApiKeyName { get; }
    public string Value { get; }

    public ApiKey(ApiKeyId apiKeyId, ApiKeyName apiKeyName, string value, UserId userId)
    {
        ArgumentNullException.ThrowIfNull(apiKeyId);
        ArgumentNullException.ThrowIfNull(apiKeyName);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentNullException.ThrowIfNull(userId);

        ApiKeyId = apiKeyId;
        ApiKeyName = apiKeyName;
        Value = value;
        UserId = userId;
    }

    public override string ToString() => Value;
}