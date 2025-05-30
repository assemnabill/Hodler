using MediatR;

namespace Hodler.Application.Users.Commands.RestPasswordRequest
{
    public class RestPasswordRequestCommand : IRequest<bool>
    {
        public string Email { get; init; } = null!;
        public string ClientUrl { get; init; } = null!;
    }
}
