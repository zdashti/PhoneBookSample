using FluentAssertions;
using PhoneBook.Domain.Exceptions;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Tests.Domain
{
    public class TagTests
    {
        [Fact]
        public void Tag_should_store_trimmed_value()
        {
            var tag = new Tag("  Coworker  ");
            tag.Value.Should().Be("Coworker");
        }

        [Fact]
        public void Tag_should_throw_for_empty_value()
        {
            FluentActions.Invoking(() => new Tag(" "))
                .Should().Throw<DomainException>();
        }

        [Fact]
        public void Tags_should_be_case_insensitive_equal()
        {
            new Tag("Friend").Should().Be(new Tag("friend"));
        }

        [Fact]
        public void Equals_should_return_false_for_different_values()
        {
            var t1 = new Tag("Friend");
            var t2 = new Tag("Coworker");
            t1.Equals(t2).Should().BeFalse();
        }

        [Fact]
        public void Equals_should_return_false_when_other_is_null()
        {
            var t = new Tag("Friend");
            t.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void Equals_should_return_false_when_other_is_different_type()
        {
            var t = new Tag("Friend");
            object other = "Friend";
            t.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_should_ignore_case()
        {
            var t1 = new Tag("Friend");
            var t2 = new Tag("friend");
            t1.GetHashCode().Should().Be(t2.GetHashCode());
        }

        [Fact]
        public void ToString_should_return_value()
        {
            var t = new Tag("Colleague");
            t.ToString().Should().Be("Colleague");
        }

    }
}