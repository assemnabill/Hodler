using MediatR;

namespace Hodler.Application.Users.Commands.ConfirmEmailRequest
{
    public class ConfirmEmailRequestCommand : IRequest<bool>
    {
        public string Email { get; init; } = null!;
        public string BaseUrl { get; init; } = null!;
    }
}
