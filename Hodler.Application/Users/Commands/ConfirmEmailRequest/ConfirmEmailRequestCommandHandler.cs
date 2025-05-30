using Hodler.Domain.Shared.EmailService;
using Hodler.Domain.Users.Services;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Hodler.Application.Users.Commands.ConfirmEmailRequest
{
    class ConfirmEmailRequestCommandHandler : IRequestHandler<ConfirmEmailRequestCommand, bool>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;

        public ConfirmEmailRequestCommandHandler(IAuthenticationService authenticationService, IEmailService emailService)
        {
            _authenticationService = authenticationService;
            _emailService = emailService;
        }
        public async Task<bool> Handle(ConfirmEmailRequestCommand request, CancellationToken cancellationToken)
        {
            var token = await _authenticationService.GenerateConfirmEmailRequestToken(request.Email);
            if (token is null)
                return false;
            var encodedToken = WebEncoders.Base64UrlEncode(
                   Encoding.UTF8.GetBytes(token));

            var emailSubj = "Confirm Your Email";
            var body = $"{request.BaseUrl}?email={request.Email}&token={encodedToken}";
            await _emailService.SendEmailAsync(request.Email, emailSubj, body, cancellationToken);
            return true;
        }
    }
}
