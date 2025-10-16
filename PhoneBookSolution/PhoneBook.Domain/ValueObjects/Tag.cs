using PhoneBook.Domain.Exceptions;

namespace PhoneBook.Domain.ValueObjects
{
    public sealed class Tag : IEquatable<Tag>
    {
        public string Value { get; }
        public Tag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) throw new DomainException(ErrorMessages.TagRequired);
            Value = tag.Trim();
        }

        public bool Equals(Tag? other) => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        public override bool Equals(object? obj) => Equals(obj as Tag);
        public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();
        public override string ToString() => Value;
    }
}
