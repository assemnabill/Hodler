using Hodler.Domain.User.Models;
using MediatR;

namespace Hodler.Application.User.Commands.AddApiKey;

public class AddApiKeyCommand : IRequest<bool>
{
    public ApiName ApiName { get; }
    public string Value { get; }
    public UserId UserId { get; }
    
    public AddApiKeyCommand(ApiName apiName, string value, UserId userId)
    {
        ApiName = apiName;
        Value = value;
        UserId = userId;
    }
}