using Hodler.Domain.Users.Services;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Hodler.Application.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
    {
        private readonly IAuthenticationService _authenticationService;

        public ConfirmEmailCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(request.Token);
            var originalToken = Encoding.UTF8.GetString(decodedTokenBytes);
            return await _authenticationService.ConfirmEmailAsync(request.Email, originalToken);
        }
    }
}
