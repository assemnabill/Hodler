using Hodler.Domain.Shared.EmailService;
using Hodler.Domain.Users.Ports;
using Hodler.Domain.Users.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Hodler.Application.Users.Commands.RestPasswordRequest
{
    public class RestPasswordRequestCommandHandler : IRequestHandler<RestPasswordRequestCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public RestPasswordRequestCommandHandler(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }
        public async Task<bool> Handle(RestPasswordRequestCommand request, CancellationToken cancellationToken)
        {
            var isValidEmail = EmailAddress.TryCreate(request.Email, out var email);
            if (!isValidEmail)
                return false;
            var token = await _userRepository.GenerateResetPasswordTokenAsync(email);
            if (token is null)
                return false;
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var body = $"{request.ClientUrl}?email={request.Email}&token={encodedToken}";
            var emailSubj = "Reset Your Password";
            await _emailService.SendEmailAsync(request.Email, emailSubj, body, cancellationToken);
            return true;
        }
    }
}
