using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.Ports;
using Hodler.Domain.Users.Services;
using Hodler.Domain.Users.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Hodler.Application.Users.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IdentityResult>
    {
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IdentityResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var email = new EmailAddress(request.Email);
                var phone = new PhoneNumber(request.PhoneNumber);
                var userName = new UserName(request.UserName);
                var contactInfo = new ContactInfo(userName, phone, email);
                var userId = new UserId(Guid.CreateVersion7());
                //TODO Use User Factory 
                var user = new User(userId, contactInfo, null, null);
                var result = await _userRepository.CreateAsync(user, request.Password, cancellationToken);
                return result;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed();
            }
           
        }
    }
}
