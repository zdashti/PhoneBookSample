using FluentAssertions;
using PhoneBook.Domain.Exceptions;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Tests.Domain
{
    public class PersonNameTests
    {
        [Fact]
        public void Constructor_should_set_properties_correctly()
        {
            var name = new PersonName("Ali", "Amiri");
            name.FirstName.Should().Be("Ali");
            name.LastName.Should().Be("Amiri");
        }

        [Theory]
        [InlineData("", "Amiri")]
        [InlineData("Ali", "")]
        [InlineData(" ", "Smith")]
        public void Constructor_should_throw_for_invalid_names(string first, string last)
        {
            FluentActions.Invoking(() => new PersonName(first, last))
                .Should().Throw<DomainException>();
        }

        [Fact]
        public void Equals_should_be_case_insensitive()
        {
            var n1 = new PersonName("Ali", "Amiri");
            var n2 = new PersonName("Ali", "Amiri");

            n1.Should().Be(n2);
        }

        [Fact]
        public void GetHashCode_should_match_for_equal_objects()
        {
            var n1 = new PersonName("Ali", "Amiri");
            var n2 = new PersonName("Ali", "Amiri");
            n1.GetHashCode().Should().Be(n2.GetHashCode());
        }

        [Fact]
        public void Equals_should_return_false_when_names_differ()
        {
            var n1 = new PersonName("Ali", "Amiri");
            var n2 = new PersonName("Ali", "Ahmadi");
            n1.Equals(n2).Should().BeFalse();
        }

        [Fact]
        public void Equals_should_ignore_case_differences()
        {
            var n1 = new PersonName("ali", "AMIRI");
            var n2 = new PersonName("ALI", "amiri");
            n1.Should().Be(n2);
        }

        [Fact]
        public void Equals_should_return_false_when_other_is_null()
        {
            var n1 = new PersonName("Ali", "Amiri");
            n1.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void Equals_should_return_false_when_other_is_different_type()
        {
            var n1 = new PersonName("Ali", "Amiri");
            object other = "Ali Amiri";
            n1.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void ToString_should_return_full_name()
        {
            var n = new PersonName("Ali", "Amiri");
            n.ToString().Should().Be("Ali Amiri");
        }

        [Fact]
        public void Constructor_should_trim_input_values()
        {
            var name = new PersonName(" Ali  ", "  Amiri ");
            name.FirstName.Should().Be("Ali");
            name.LastName.Should().Be("Amiri");
        }

        [Fact]
        public void GetHashCode_should_ignore_case()
        {
            var n1 = new PersonName("Ali", "Amiri");
            var n2 = new PersonName("ali", "amiri");
            n1.GetHashCode().Should().Be(n2.GetHashCode());
        }
    }
}