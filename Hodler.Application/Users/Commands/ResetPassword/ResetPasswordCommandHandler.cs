using Hodler.Domain.Users.Ports;
using Hodler.Domain.Users.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Hodler.Application.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var isValidEmail = EmailAddress.TryCreate(request.Email, out var email);
            if (!isValidEmail)
                return false;
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(request.Token);
            var originalToken = Encoding.UTF8.GetString(decodedTokenBytes);
            return await _userRepository.ResetPasswordAsync(email, request.NewPassword, originalToken);
        }
    }
}
