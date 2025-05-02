using Hodler.Domain.CryptoExchanges.Models;

namespace Hodler.Domain.Users.Models;

public class ApiKey
{
    public ApiKeyId ApiKeyId { get; }
    public UserId UserId { get; }
    public ApiKeyName ApiKeyName { get; }
    public string Value { get; }
    public string? Secret { get; }

    public ApiKey(
        ApiKeyId apiKeyId,
        ApiKeyName apiKeyName,
        string value,
        UserId userId,
        string? secret = null
    )
    {
        ArgumentNullException.ThrowIfNull(apiKeyId);
        ArgumentNullException.ThrowIfNull(apiKeyName);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentNullException.ThrowIfNull(userId);

        ApiKeyId = apiKeyId;
        ApiKeyName = apiKeyName;
        Value = value;
        UserId = userId;
        Secret = secret;

        EnsureSecretIsPresentWhenRequired();
    }

    private void EnsureSecretIsPresentWhenRequired()
    {
        var isSecretRequired = ApiKeyName switch
        {
            ApiKeyName.Kraken => true,
            _ => false
        };

        if (isSecretRequired && string.IsNullOrWhiteSpace(Secret))
        {
            throw new ArgumentException($"Secret is required for this Api ({ApiKeyName})");
        }
    }

    public override string ToString() => Value;
}