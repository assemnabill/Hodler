using Hodler.Domain.Shared.EmailService;
using Hodler.Domain.Users.Services;
using MediatR;

namespace Hodler.Application.Users.Commands.RestPasswordRequest
{
    public class RestPasswordRequestCommandHandler : IRequestHandler<RestPasswordRequestCommand, bool>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;

        public RestPasswordRequestCommandHandler(IAuthenticationService authenticationService, IEmailService emailService)
        {
            _authenticationService = authenticationService;
            _emailService = emailService;
        }
        public async Task<bool> Handle(RestPasswordRequestCommand request, CancellationToken cancellationToken)
        {
            var token = await _authenticationService.GenerateResetPasswordRequestToken(request.Email);
            if (token is null)
                return false;
            var body = $"{request.ClientUrl}?email={request.Email}&token={token}";
            var emailSubj = "Reset Your Password";
            await _emailService.SendEmailAsync(request.Email, emailSubj, body, cancellationToken);
            return true;
        }
    }
}
