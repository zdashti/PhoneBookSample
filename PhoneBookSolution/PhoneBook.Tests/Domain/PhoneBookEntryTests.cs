using FluentAssertions;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Exceptions;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Tests.Domain
{
    public class PhoneBookEntryTests
    {
        [Fact]
        public void Create_should_initialize_all_properties()
        {
            var name = new PersonName("Ali", "Amiri");
            var phone = new PhoneNumber("09123456789");
            var tag = new Tag("Friend");

            var entry = PhoneBookEntry.Create(name, phone, tag);

            entry.Id.Should().NotBe(Guid.Empty);
            entry.Name.Should().Be(name);
            entry.PhoneNumber.Should().Be(phone);
            entry.Tag.Should().Be(tag);
            entry.CreatedAt.Should().NotBe(default);
            entry.UpdatedAt.Should().NotBe(default);
        }

        [Fact]
        public void Create_should_throw_when_tag_parameter_is_null()
        {
            var name = new PersonName("Ali", "Amiri");
            var phone = new PhoneNumber("09123456789");
            Tag? tag = null;

            FluentActions.Invoking(() => PhoneBookEntry.Create(name!, phone!, tag!))
                .Should().Throw<DomainException>()
                .WithMessage("*required*");
        }
        [Fact]
        public void Create_should_throw_when_phone_parameter_is_null()
        {
            var name = new PersonName("Ali", "Amiri");
            PhoneNumber? phone = null;
            var tag = new Tag("Friend");

            FluentActions.Invoking(() => PhoneBookEntry.Create(name!, phone!, tag!))
                .Should().Throw<DomainException>()
                .WithMessage("*required*");
        }
        [Fact]
        public void Create_should_throw_when_name_parameter_is_null()
        {
            PersonName? name = null;
            var phone = new PhoneNumber("09123456789");
            var tag = new Tag("Friend");

            FluentActions.Invoking(() => PhoneBookEntry.Create(name!, phone!, tag!))
                .Should().Throw<DomainException>()
                .WithMessage("*required*");
        }

        [Fact]
        public void UpdateName_should_change_name_and_update_timestamp()
        {
            var entry = PhoneBookEntry.Create(
                new PersonName("Ali", "Amiri"),
                new PhoneNumber("09123456789"),
                new Tag("Friend")
            );
            var oldUpdated = entry.UpdatedAt;

            var newName = new PersonName("Sara", "Ahmadi");
            entry.UpdateName(newName);

            entry.Name.Should().Be(newName);
            entry.UpdatedAt.Should().BeAfter(oldUpdated);
        }

        [Fact]
        public void UpdateName_should_throw_when_null()
        {
            var entry = PhoneBookEntry.Create(
                new PersonName("Ali", "Amiri"),
                new PhoneNumber("09123456789"),
                new Tag("Friend")
            );

            FluentActions.Invoking(() => entry.UpdateName(null!))
                .Should().Throw<DomainException>()
                .WithMessage("*Name is required*");
        }

        [Fact]
        public void UpdatePhoneNumber_should_change_number_and_update_timestamp()
        {
            var entry = PhoneBookEntry.Create(
                new PersonName("Ali", "Amiri"),
                new PhoneNumber("09123456789"),
                new Tag("Friend")
            );
            var oldUpdated = entry.UpdatedAt;

            var newNumber = new PhoneNumber("02155512345");
            entry.UpdatePhoneNumber(newNumber);

            entry.PhoneNumber.Should().Be(newNumber);
            entry.UpdatedAt.Should().BeAfter(oldUpdated);
        }

        [Fact]
        public void UpdatePhoneNumber_should_throw_when_null()
        {
            var entry = PhoneBookEntry.Create(
                new PersonName("Ali", "Amiri"),
                new PhoneNumber("09123456789"),
                new Tag("Friend")
            );

            FluentActions.Invoking(() => entry.UpdatePhoneNumber(null!))
                .Should().Throw<DomainException>()
                .WithMessage("*Phone number is required*");
        }

        [Fact]
        public void UpdateTag_should_change_tag_and_update_timestamp()
        {
            var entry = PhoneBookEntry.Create(
                new PersonName("Ali", "Amiri"),
                new PhoneNumber("09123456789"),
                new Tag("Friend")
            );
            var oldUpdated = entry.UpdatedAt;

            var newTag = new Tag("Coworker");
            entry.UpdateTag(newTag);

            entry.Tag.Should().Be(newTag);
            entry.UpdatedAt.Should().BeAfter(oldUpdated);
        }

        [Fact]
        public void UpdateTag_should_throw_when_null()
        {
            var entry = PhoneBookEntry.Create(
                new PersonName("Ali", "Amiri"),
                new PhoneNumber("09123456789"),
                new Tag("Friend")
            );

            FluentActions.Invoking(() => entry.UpdateTag(null!))
                .Should().Throw<DomainException>()
                .WithMessage("*Tag is required*");
        }


    }
}
