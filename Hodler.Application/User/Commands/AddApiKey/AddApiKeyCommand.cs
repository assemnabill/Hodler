using Hodler.Domain.CryptoExchange.Models;
using Hodler.Domain.User.Models;
using MediatR;

namespace Hodler.Application.User.Commands.AddApiKey;

public class AddApiKeyCommand : IRequest<bool>
{
    public ApiKeyName ApiKeyName { get; }
    public string Value { get; }
    public UserId UserId { get; }
    public string? Secret { get; }

    public AddApiKeyCommand(ApiKeyName apiKeyName, string value, UserId userId, string? secret)
    {
        ArgumentNullException.ThrowIfNull(apiKeyName);
        ArgumentException.ThrowIfNullOrEmpty(value);
        ArgumentNullException.ThrowIfNull(userId);

        ApiKeyName = apiKeyName;
        Value = value;
        UserId = userId;
        Secret = secret;
    }
}