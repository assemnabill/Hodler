
using Hodler.Domain.Users.ValueObjects;

namespace Hodler.Domain.Users.Models
{
    public class ContactInfo
    {
        public ContactInfo(UserName userName, PhoneNumber phoneNumber, EmailAddress email)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }

        public UserName UserName { get; } = null!;
        public PhoneNumber PhoneNumber { get; } = null!;
        public EmailAddress Email { get; } = null!;

    }
}
