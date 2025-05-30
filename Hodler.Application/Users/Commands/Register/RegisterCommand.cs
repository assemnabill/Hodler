using Hodler.Domain.Users.AuthenticationResult;
using MediatR;

namespace Hodler.Application.Users.Commands.Register
{
    public class RegisterCommand : IRequest<RegisterResult>
    {
        public required string Email { get; init; }
        public required string UserName { get; init; }
        public required string PhoneNumber { get; init; }
        public required string Password { get; init; }
    }
}
