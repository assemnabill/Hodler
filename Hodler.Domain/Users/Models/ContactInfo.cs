
namespace Hodler.Domain.Users.Models
{
    public class ContactInfo
    {
        public ContactInfo(string userName, string phoneNumber, string email)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }

        public string UserName { get; } = null!;
        public string PhoneNumber { get; } = null!;
        public string Email { get; } = null!;

    }
}
