using Hodler.Domain.Users.AuthenticationResult;
using Hodler.Domain.Users.Services;
using MediatR;

namespace Hodler.Application.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _authenticationService.LoginAsync(request.UserNameOrEmail, request.Password, cancellationToken);
        }
    }
}
