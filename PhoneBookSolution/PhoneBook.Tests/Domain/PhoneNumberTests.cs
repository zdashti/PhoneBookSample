using FluentAssertions;
using PhoneBook.Domain.Constants;
using PhoneBook.Domain.Exceptions;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Tests.Domain
{
    public class PhoneNumberTests
    {
        [Theory]
        [InlineData("+98 (21) 1234567", "98211234567")]
        [InlineData("0912 345 6789", "09123456789")]
        [InlineData("021-55512345", "02155512345")]
        public void Constructor_should_normalize_and_accept_valid_numbers(string raw, string expectedDigits)
        {
            var pn = new PhoneNumber(raw);
            var digits = System.Text.RegularExpressions.Regex.Replace(pn.Value, RegexPatterns.NonDigit, "");
            digits.Should().Be(expectedDigits);
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("1234567890121")]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_should_throw_for_invalid_numbers(string raw)
        {
            FluentActions.Invoking(() => new PhoneNumber(raw))
                .Should().Throw<DomainException>()
                .Where(e => e.Message.Contains("invalid", StringComparison.OrdinalIgnoreCase)
                            || e.Message.Contains("required", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Equals_should_compare_digits_only()
        {
            var p1 = new PhoneNumber("+98 (21) 1234567");
            var p2 = new PhoneNumber("98211234567");
            p1.Should().Be(p2);
        }

        [Fact]
        public void GetHashCode_should_match_for_equivalent_numbers()
        {
            var p1 = new PhoneNumber("0912-345-6789");
            var p2 = new PhoneNumber("09123456789");
            p1.GetHashCode().Should().Be(p2.GetHashCode());
        }

        [Fact]
        public void Equals_should_return_false_for_different_numbers()
        {
            var p1 = new PhoneNumber("09123456789");
            var p2 = new PhoneNumber("09351234567");

            p1.Equals(p2).Should().BeFalse();
            (p1 == p2).Should().BeFalse();
        }

        [Fact]
        public void Equals_should_return_false_when_other_is_null_or_different_type()
        {
            var p1 = new PhoneNumber("09123456789");

            p1.Equals(null).Should().BeFalse();
            p1.Equals("09123456789").Should().BeFalse();
        }

        [Fact]
        public void ToString_should_return_original_value()
        {
            var raw = "+98 (912) 345-6789";
            var pn = new PhoneNumber(raw);
            pn.ToString().Should().Be(pn.Value);
        }

        [Fact]
        public void Constructing_from_existing_value_should_keep_same_digits()
        {
            var first = new PhoneNumber("0912-345-6789");
            var second = new PhoneNumber(first.Value);

            var d1 = System.Text.RegularExpressions.Regex.Replace(first.Value, RegexPatterns.NonDigit, "");
            var d2 = System.Text.RegularExpressions.Regex.Replace(second.Value, RegexPatterns.NonDigit, "");

            d1.Should().Be(d2);
        }

        [Fact]
        public void Constructor_should_throw_InvalidPhoneNumber_message_for_short_or_long_input()
        {
            var ex = FluentActions.Invoking(() => new PhoneNumber("12345"))
                .Should().Throw<DomainException>().Which;

            ex.Message.Should().Be(ErrorMessages.InvalidPhoneNumber);
        }

        [Fact]
        public void Constructor_should_throw_PhoneNumberRequired_message_for_empty_input()
        {
            var ex = FluentActions.Invoking(() => new PhoneNumber("   "))
                .Should().Throw<DomainException>().Which;

            ex.Message.Should().Be(ErrorMessages.PhoneNumberRequired);
        }

        [Fact]
        public void Operator_equals_and_not_equals_should_work_correctly()
        {
            var p1 = new PhoneNumber("09123456789");
            var p2 = new PhoneNumber("0912-345-6789");
            var p3 = new PhoneNumber("09351234567");

            (p1 == p2).Should().BeTrue();
            (p1 != p3).Should().BeTrue();
            (p1 == null).Should().BeFalse();
            (null == p1).Should().BeFalse();
            (null == (PhoneNumber?)null).Should().BeTrue();
        }

    }
}