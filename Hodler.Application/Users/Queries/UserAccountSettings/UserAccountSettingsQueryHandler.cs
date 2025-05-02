using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Users.Queries.UserAccountSettings;

public class UserAccountSettingsQueryHandler
    : IRequestHandler<UserAccountSettingsQuery, UserSettings>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UserAccountSettingsQueryHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<UserSettings> Handle(
        UserAccountSettingsQuery request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var service = _serviceScopeFactory
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<IUserSettingsQueryService>();

        var userSettings = await service.GetAccountSettings(request.UserId, cancellationToken);

        return userSettings;
    }
}