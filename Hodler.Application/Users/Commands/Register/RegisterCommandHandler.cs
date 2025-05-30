using Hodler.Domain.Users.AuthenticationResult;
using Hodler.Domain.Users.Services;
using MediatR;

namespace Hodler.Application.Users.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResult>
    {
        private readonly IAuthenticationService _authenticationService;

        public RegisterCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var newUser = await _authenticationService.CreateUserAsync(request.Email, request.Password
                                                                       , request.UserName, request.PhoneNumber,
                                                                       cancellationToken);
            return newUser;
        }
    }
}
