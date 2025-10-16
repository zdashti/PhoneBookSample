using PhoneBook.Domain.Exceptions;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Domain.Entities
{
    public class PhoneBookEntry
    {
        public Guid Id { get; private set; }
        public PersonName Name { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public Tag Tag { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private PhoneBookEntry() { }

        public static PhoneBookEntry Create(PersonName? name, PhoneNumber? phoneNumber, Tag? tag)
        {
            if (name is null) throw new DomainException("Name is required");
            if (phoneNumber is null) throw new DomainException("Phone number is required");
            if (tag is null) throw new DomainException("Tag is required");

            var e = new PhoneBookEntry
            {
                Id = Guid.NewGuid(),
                Name = name,
                PhoneNumber = phoneNumber,
                Tag = tag,
                CreatedAt = DomainDateTime.Now(),
                UpdatedAt = DomainDateTime.Now(),
            };
            return e;
        }

        public void UpdateName(PersonName newName)
        {
            Name = newName ?? throw new DomainException("Name is required");
            UpdatedAt = DomainDateTime.Now();
        }

        public void UpdatePhoneNumber(PhoneNumber newNumber)
        {
            PhoneNumber = newNumber ?? throw new DomainException("Phone number is required");
            UpdatedAt = DomainDateTime.Now();
        }

        public void UpdateTag(Tag newTag)
        {
            Tag = newTag ?? throw new DomainException("Tag is required");
            UpdatedAt = DomainDateTime.Now();
        }
    }
}