using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Users.Queries.UserAccountSettings;

public class UserAccountSettingsQuery : IRequest<UserSettings>
{
    public UserId UserId { get; }

    public UserAccountSettingsQuery(UserId userId)
    {
        UserId = userId;
    }
}