using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Users.Commands.AddApiKey;

public class AddApiKeyCommand : IRequest<bool>
{
    public ApiKeyName ApiKeyName { get; }
    public string Value { get; }
    public UserId UserId { get; }
    public string? Secret { get; }

    public AddApiKeyCommand(ApiKeyName apiKeyName, string value, UserId userId, string? secret)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        ArgumentNullException.ThrowIfNull(userId);

        ApiKeyName = apiKeyName;
        Value = value;
        UserId = userId;
        Secret = secret;
    }
}