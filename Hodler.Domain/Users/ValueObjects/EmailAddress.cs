using System.Text.RegularExpressions;

namespace Hodler.Domain.Users.ValueObjects
{
    public sealed class EmailAddress : IEquatable<EmailAddress>
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; }

        public EmailAddress(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email address cannot be null or empty.", nameof(email));

            var trimmedEmail = email.Trim();

            if (!IsValidEmail(trimmedEmail))
                throw new ArgumentException($"Invalid email address format: {trimmedEmail}", nameof(email));

            Value = trimmedEmail.ToLowerInvariant();
        }

        private static bool IsValidEmail(string email)
        {
            if (email.Length > 254) 
                return false;

            return EmailRegex.IsMatch(email);
        }

        public string GetDomain()
        {
            var atIndex = Value.LastIndexOf('@');
            return atIndex > 0 ? Value.Substring(atIndex + 1) : string.Empty;
        }

        public string GetLocalPart()
        {
            var atIndex = Value.LastIndexOf('@');
            return atIndex > 0 ? Value.Substring(0, atIndex) : string.Empty;
        }
        
        public bool Equals(EmailAddress other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EmailAddress);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return Value;
        }
        
        public static bool operator ==(EmailAddress left, EmailAddress right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EmailAddress left, EmailAddress right)
        {
            return !Equals(left, right);
        }
        public static implicit operator string(EmailAddress emailAddress)
        {
            return emailAddress?.Value;
        }
        
        public static EmailAddress Create(string email)
        {
            return new EmailAddress(email);
        }
        
        public static bool TryCreate(string email, out EmailAddress? emailAddress)
        {
            try
            {
                emailAddress = new EmailAddress(email);
                return true;
            }
            catch
            {
                emailAddress = null;
                return false;
            }
        }
    }
}
