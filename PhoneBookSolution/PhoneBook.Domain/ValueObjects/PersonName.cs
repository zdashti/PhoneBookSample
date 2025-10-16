using PhoneBook.Domain.Exceptions;

namespace PhoneBook.Domain.ValueObjects
{
    public sealed class PersonName : IEquatable<PersonName>
    {
        public string FirstName { get; }
        public string LastName { get; }

        public PersonName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new DomainException(ErrorMessages.FirstNameRequired);
            if (string.IsNullOrWhiteSpace(lastName)) throw new DomainException(ErrorMessages.LastNameRequired);

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        public bool Equals(PersonName? other)
        {
            if (other is null) return false;
            return string.Equals(FirstName, other.FirstName, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(LastName, other.LastName, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj) => Equals(obj as PersonName);
        public override int GetHashCode() => HashCode.Combine(FirstName.ToLowerInvariant(), LastName.ToLowerInvariant());

        public override string ToString() => $"{FirstName} {LastName}";
    }
}
