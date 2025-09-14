using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Hodler.Application.Users.Commands.Register
{
    public class RegisterCommand : IRequest<IdentityResult>
    {
        public required string Email { get; init; }
        public required string UserName { get; init; }
        public required string PhoneNumber { get; init; }
        public required string Password { get; init; }
    }
}
