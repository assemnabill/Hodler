using Hodler.Domain.Users.AuthenticationResult;
using MediatR;

namespace Hodler.Application.Users.Commands.Login
{
    public class LoginCommand : IRequest<LoginResult>
    {
        public required string UserNameOrEmail { get; init; } = null!;
        public required string Password { get; init; } = null!;
    }
}
