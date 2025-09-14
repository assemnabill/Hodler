using Hodler.Domain.Shared.EmailService;
using Hodler.Domain.Users.Ports;
using Hodler.Domain.Users.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Hodler.Application.Users.Commands.ConfirmEmailRequest
{
    class ConfirmEmailRequestCommandHandler : IRequestHandler<ConfirmEmailRequestCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ConfirmEmailRequestCommandHandler(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }
        public async Task<bool> Handle(ConfirmEmailRequestCommand request, CancellationToken cancellationToken)
        {
            var isValidEmail = EmailAddress.TryCreate(request.Email, out var email);
            if (!isValidEmail)
                return false;
            var token = await _userRepository.GenerateConfirmEmailTokenAsync(email);
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
