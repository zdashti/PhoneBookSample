using FluentAssertions;
using Moq;
using PhoneBook.Application.DTOs;
using PhoneBook.Application.Services;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Exceptions;
using PhoneBook.Domain.Interfaces;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Tests.Application
{
    public class PhoneBookServiceTests
    {
        private readonly Mock<IPhoneBookRepository> _repoMock;
        private readonly PhoneBookService _service;

        public PhoneBookServiceTests()
        {
            _repoMock = new Mock<IPhoneBookRepository>();
            _service = new PhoneBookService(_repoMock.Object);
        }

        [Fact]
        public async Task AddAsync_should_create_and_return_entry_dto()
        {
            // Arrange
            var dto = new CreateEntryDto("Ali", "Rezaei", "09123456789", "Friend");

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            result.FirstName.Should().Be("Ali");
            result.LastName.Should().Be("Rezaei");
            result.PhoneNumber.Should().Contain("09123456789");
            result.Tag.Should().Be("Friend");

            _repoMock.Verify(r => r.AddAsync(It.IsAny<PhoneBookEntry>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_should_update_existing_entry()
        {
            // Arrange
            var entry = PhoneBookEntry.Create(
                new PersonName("Ali", "Rezaei"),
                new PhoneNumber("09123456789"),
                new Tag("Friend"));

            _repoMock.Setup(r => r.GetByIdAsync(entry.Id))
                .ReturnsAsync(entry);

            var updateDto = new UpdateEntryDto(entry.Id, "Hossein", null, null, null);

            // Act
            var result = await _service.UpdateAsync(updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.FirstName.Should().Be("Hossein");
            result.LastName.Should().Be("Rezaei");
            _repoMock.Verify(r => r.UpdateAsync(entry), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_should_return_null_if_entry_not_found()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((PhoneBookEntry?)null);

            var dto = new UpdateEntryDto(Guid.NewGuid(), "A", "B", "09120000000", "Friend");

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_should_return_true_if_deleted()
        {
            _repoMock.Setup(r => r.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var ok = await _service.DeleteAsync(Guid.NewGuid());

            ok.Should().BeTrue();
        }

        [Fact]
        public async Task GetByTagAsync_should_return_entries_with_tag()
        {
            var entry1 = PhoneBookEntry.Create(new PersonName("Ali", "Rezaei"), new PhoneNumber("09123456789"), new Tag("Friend"));
            var entry2 = PhoneBookEntry.Create(new PersonName("Sara", "Karimi"), new PhoneNumber("09351234567"), new Tag("Friend"));
            var entry3 = PhoneBookEntry.Create(new PersonName("Omid", "Naseri"), new PhoneNumber("09998887777"), new Tag("Coworker"));

            _repoMock.Setup(r => r.GetByTagAsync("Friend"))
                .ReturnsAsync(new List<PhoneBookEntry> { entry1, entry2 });

            var result = await _service.GetByTagAsync("Friend");

            result.Should().HaveCount(2);
            result.All(e => e.Tag == "Friend").Should().BeTrue();
        }

        [Fact]
        public async Task GetAllAsync_should_return_all_entries()
        {
            var entries = new List<PhoneBookEntry>
            {
                PhoneBookEntry.Create(new PersonName("Ali", "Rezaei"), new PhoneNumber("09123456789"), new Tag("Friend")),
                PhoneBookEntry.Create(new PersonName("Sara", "Karimi"), new PhoneNumber("09351234567"), new Tag("Coworker"))
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entries);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result.Select(r => r.FirstName).Should().Contain(new[] { "Ali", "Sara" });
        }

        [Fact]
        public async Task GetByIdAsync_should_return_entry_if_found()
        {
            var entry = PhoneBookEntry.Create(
                new PersonName("Ali", "Rezaei"),
                new PhoneNumber("09123456789"),
                new Tag("Friend"));

            _repoMock.Setup(r => r.GetByIdAsync(entry.Id)).ReturnsAsync(entry);

            var result = await _service.GetByIdAsync(entry.Id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(entry.Id);
        }

        [Fact]
        public async Task GetByIdAsync_should_return_null_if_not_found()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((PhoneBookEntry?)null);

            var result = await _service.GetByIdAsync(Guid.NewGuid());

            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_should_throw_when_input_is_invalid()
        {
            var invalidDto = new CreateEntryDto("", "", "abc", " ");

            await FluentActions.Invoking(() => _service.AddAsync(invalidDto))
                .Should().ThrowAsync<DomainException>();
        }

        [Fact]
        public async Task UpdateAsync_should_update_phone_number_only()
        {
            var entry = PhoneBookEntry.Create(new PersonName("Ali", "Rezaei"), new PhoneNumber("09123456789"), new Tag("Friend"));
            _repoMock.Setup(r => r.GetByIdAsync(entry.Id)).ReturnsAsync(entry);

            var dto = new UpdateEntryDto(entry.Id, null, null, "09998887777", null);

            var result = await _service.UpdateAsync(dto);

            result!.PhoneNumber.Should().Contain("09998887777");
            result.FirstName.Should().Be("Ali");
            _repoMock.Verify(r => r.UpdateAsync(entry), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_should_return_false_if_not_deleted()
        {
            _repoMock.Setup(r => r.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            var ok = await _service.DeleteAsync(Guid.NewGuid());

            ok.Should().BeFalse();
        }

        [Fact]
        public async Task AddAsync_should_map_created_and_updated_dates()
        {
            var dto = new CreateEntryDto("Ali", "Rezaei", "09123456789", "Friend");

            var result = await _service.AddAsync(dto);

            result.CreatedAt.Should().NotBe(default);
            result.UpdatedAt.Should().NotBe(default);
        }

    }
}
