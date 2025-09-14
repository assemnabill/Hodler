using System.Text.RegularExpressions;

namespace Hodler.Domain.Users.ValueObjects
{
    public sealed class PhoneNumber
    {
        private static readonly Regex PhoneRegex = new Regex(
            @"^\+?[1-9]\d{1,14}$",
            RegexOptions.Compiled);

        private static readonly Regex DigitsOnlyRegex = new Regex(
            @"[^\d+]",
            RegexOptions.Compiled);

        public string Value { get; }
        public string CountryCode { get; }
        public string NationalNumber { get; }

        public PhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be null or empty.", nameof(phoneNumber));

            var normalizedNumber = NormalizePhoneNumber(phoneNumber);

            if (!IsValidPhoneNumber(normalizedNumber))
                throw new ArgumentException($"Invalid phone number format: {phoneNumber}", nameof(phoneNumber));

            Value = normalizedNumber;
            (CountryCode, NationalNumber) = ExtractParts(normalizedNumber);
        }

        private static string NormalizePhoneNumber(string phoneNumber)
        {
            // Remove all non-digit characters except +
            var cleaned = DigitsOnlyRegex.Replace(phoneNumber.Trim(), "");

            // Ensure it starts with + if it doesn't already
            if (!cleaned.StartsWith("+"))
            {
                cleaned = "+" + cleaned;
            }

            return cleaned;
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Must match E.164 format: + followed by 1-15 digits
            if (!PhoneRegex.IsMatch(phoneNumber))
                return false;

            // Length check (1-15 digits after +)
            var digitsOnly = phoneNumber.Substring(1);
            return digitsOnly.Length >= 4 && digitsOnly.Length <= 15;
        }

        private static (string countryCode, string nationalNumber) ExtractParts(string phoneNumber)
        {
            var digitsOnly = phoneNumber.Substring(1); // Remove +

            // Simple heuristic for common country codes
            // In a real implementation, you'd use a proper country code library
            if (digitsOnly.StartsWith("1") && digitsOnly.Length == 11) // NANP (US, Canada)
                return ("1", digitsOnly.Substring(1));
            else if (digitsOnly.StartsWith("44")) // UK
                return ("44", digitsOnly.Substring(2));
            else if (digitsOnly.StartsWith("49")) // Germany
                return ("49", digitsOnly.Substring(2));
            else if (digitsOnly.StartsWith("33")) // France
                return ("33", digitsOnly.Substring(2));
            else if (digitsOnly.StartsWith("81")) // Japan
                return ("81", digitsOnly.Substring(2));
            else if (digitsOnly.StartsWith("86")) // China
                return ("86", digitsOnly.Substring(2));
            else if (digitsOnly.StartsWith("91")) // India
                return ("91", digitsOnly.Substring(2));
            else if (digitsOnly.StartsWith("20")) // Egypt
                return ("20", digitsOnly.Substring(2));
            else
            {
                // Default: assume first 1-3 digits are country code
                var countryCodeLength = digitsOnly.Length > 10 ? 2 : 1;
                return (digitsOnly.Substring(0, countryCodeLength),
                       digitsOnly.Substring(countryCodeLength));
            }
        }
    }
}
