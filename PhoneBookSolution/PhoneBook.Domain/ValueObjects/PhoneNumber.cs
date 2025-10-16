using PhoneBook.Domain.Constants;
using PhoneBook.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace PhoneBook.Domain.ValueObjects
{
    public sealed class PhoneNumber : IEquatable<PhoneNumber>
    {
        public string Value { get; }

        private static readonly Regex Sanitizer = new(@"[^\d\+]+", RegexOptions.Compiled);

        public PhoneNumber(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) throw new DomainException(ErrorMessages.InvalidPhoneNumber);
            var cleaned = Sanitizer.Replace(raw.Trim(), "");

            var digitsOnly = Regex.Replace(cleaned, RegexPatterns.NonDigit, "");
            if (digitsOnly.Length is < ValidationRules.PhoneNumberMinLength or > ValidationRules.PhoneNumberMaxLength)
                throw new DomainException(ErrorMessages.PhoneNumberRequired);

            Value = cleaned;
        }

        public bool Equals(PhoneNumber? other)
        {
            if (other is null) return false;

            var d1 = Regex.Replace(Value, RegexPatterns.NonDigit, "");
            var d2 = Regex.Replace(other.Value, RegexPatterns.NonDigit, "");
            return d1 == d2;
        }

        public static bool operator ==(PhoneNumber? left, PhoneNumber? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(PhoneNumber? left, PhoneNumber? right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj) => Equals(obj as PhoneNumber);
        public override int GetHashCode() => Regex.Replace(Value, RegexPatterns.NonDigit, "").GetHashCode();

        public override string ToString() => Value;
    }
}
