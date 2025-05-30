using MediatR;

namespace Hodler.Application.Users.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public string Email { get; init; } = null!;
        public string Token { get; init; } = null!;
        public string NewPassword { get; init; } = null!;
    }
}
