using MediatR;

namespace Hodler.Application.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<bool>
    {
        public string Email { get; init; } = null!;
        public string Token { get; init; } = null!;
    }
}
