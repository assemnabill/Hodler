using System.Text.RegularExpressions;

namespace Hodler.Domain.Users.ValueObjects
{
    public sealed class UserName : IEquatable<UserName>
    {
        private static readonly Regex UserNameRegex = new Regex(
            @"^[a-zA-Z0-9][a-zA-Z0-9._-]*[a-zA-Z0-9]$",
            RegexOptions.Compiled);

        private static readonly Regex SingleCharRegex = new Regex(
            @"^[a-zA-Z0-9]$",
            RegexOptions.Compiled);

        private static readonly string[] ReservedUserNames =
        {
           "admin", "administrator", "root", "system", "user", "guest", "test",
           "support", "help", "api", "www", "mail", "email", "ftp", "http",
           "https", "ssl", "tls", "about", "contact", "privacy", "terms",
           "login", "register", "signup", "signin", "logout", "null", "undefined"
        };

        private static readonly char[] InvalidConsecutiveChars = { '.', '_', '-' };

        public string Value { get; }
        public int Length => Value.Length;

        public UserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Username cannot be null or empty.", nameof(userName));

            var trimmedUserName = userName.Trim();

            if (!IsValidUserName(trimmedUserName))
                throw new ArgumentException($"Invalid username format: {trimmedUserName}", nameof(userName));

            if (IsReservedUserName(trimmedUserName))
                throw new ArgumentException($"Username '{trimmedUserName}' is reserved and cannot be used.", nameof(userName));

            Value = trimmedUserName.ToLowerInvariant();
        }

        private static bool IsValidUserName(string userName)
        {
            // Length validation (3-30 characters)
            if (userName.Length < 3 || userName.Length > 30)
                return false;
            // Multi-character username validation
            if (!UserNameRegex.IsMatch(userName))
                return false;
            return true;
        }

        private static bool IsReservedUserName(string userName)
        {
            return ReservedUserNames.Contains(userName.ToLowerInvariant());
        }
        public bool Equals(UserName other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserName);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return Value;
        }

        // Operators
        public static bool operator ==(UserName left, UserName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserName left, UserName right)
        {
            return !Equals(left, right);
        }
        public static implicit operator string(UserName userName)
        {
            return userName?.Value;
        }

        public static UserName Create(string userName)
        {
            return new UserName(userName);
        }

        public static bool TryCreate(string userName, out UserName? result)
        {
            try
            {
                result = new UserName(userName);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}
