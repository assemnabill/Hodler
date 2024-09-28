using Hodler.Domain.User.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.User.Commands.AddApiKey;

public class AddApiKeyCommandHandler : IRequestHandler<AddApiKeyCommand, bool>
{
    private readonly IServiceProvider _serviceProvider;

    public AddApiKeyCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<bool> Handle(AddApiKeyCommand request, CancellationToken cancellationToken)
    {
        var domainService = _serviceProvider.GetRequiredService<IUserSettingsService>();

        var result = await domainService.AddApiKeyAsync(
            request.ApiKeyName,
            request.Value,
            request.UserId,
            cancellationToken
        );

        return result;
    }
}