using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PhoneBook.Api.Controllers;
using PhoneBook.Application.DTOs;
using PhoneBook.Application.Services;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Tests.Api
{
    public class PhoneBookControllerTests
    {
        [Fact]
        public async Task Create_returns_CreatedAtAction()
        {
            var svc = new Mock<IPhoneBookService>();
            var dto = new CreateEntryDto("J", "K", "021-22547859", "Tag");
            var returned = new EntryDto(Guid.NewGuid(), "J", "K", "22547859", "Tag", DomainDateTime.Now(), DomainDateTime.Now());
            svc.Setup(s => s.AddAsync(dto)).ReturnsAsync(returned);

            var controller = new PhoneBookController(svc.Object);
            var result = await controller.Create(dto);
            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_should_return_500_if_service_throws()
        {
            var svc = new Mock<IPhoneBookService>();
            svc.Setup(s => s.AddAsync(It.IsAny<CreateEntryDto>()))
                .ThrowsAsync(new Exception("unexpected"));

            var controller = new PhoneBookController(svc.Object);
            Func<Task> act = async () => await controller.Create(new CreateEntryDto("A", "B", "09123456789", "T"));

            await act.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task GetById_should_return_Ok_when_entry_exists()
        {
            var svc = new Mock<IPhoneBookService>();
            var entry = new EntryDto(Guid.NewGuid(), "Ali", "Rezaei", "09123456789", "Friend", DomainDateTime.Now(), DomainDateTime.Now());
            svc.Setup(s => s.GetByIdAsync(entry.Id)).ReturnsAsync(entry);

            var controller = new PhoneBookController(svc.Object);
            var result = await controller.GetById(entry.Id);

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be(entry);
        }

        [Fact]
        public async Task GetById_should_return_NotFound_when_entry_missing()
        {
            var svc = new Mock<IPhoneBookService>();
            svc.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((EntryDto?)null);

            var controller = new PhoneBookController(svc.Object);
            var result = await controller.GetById(Guid.NewGuid());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Update_should_return_BadRequest_when_id_mismatch()
        {
            var svc = new Mock<IPhoneBookService>();
            var controller = new PhoneBookController(svc.Object);
            var dto = new UpdateEntryDto(Guid.NewGuid(), "A", "B", "09123456789", "Friend");

            var result = await controller.Update(Guid.NewGuid(), dto);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Update_should_return_NotFound_when_service_returns_null()
        {
            var svc = new Mock<IPhoneBookService>();
            svc.Setup(s => s.UpdateAsync(It.IsAny<UpdateEntryDto>())).ReturnsAsync((EntryDto?)null);

            var controller = new PhoneBookController(svc.Object);
            var dto = new UpdateEntryDto(Guid.NewGuid(), "A", "B", "09123456789", "Friend");

            var result = await controller.Update(dto.Id, dto);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Update_should_return_Ok_when_successful()
        {
            var svc = new Mock<IPhoneBookService>();
            var dto = new UpdateEntryDto(Guid.NewGuid(), "Ali", "Rezaei", "09123456789", "Friend");
            var returned = new EntryDto(dto.Id, "Ali", "Rezaei", "09123456789", "Friend", DomainDateTime.Now(), DomainDateTime.Now());
            svc.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(returned);

            var controller = new PhoneBookController(svc.Object);
            var result = await controller.Update(dto.Id, dto);

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be(returned);
        }

        [Fact]
        public async Task GetByTag_should_return_Ok_with_results()
        {
            var svc = new Mock<IPhoneBookService>();
            var list = new List<EntryDto>
            {
                new(Guid.NewGuid(), "Ali", "A", "0912", "Friend", DomainDateTime.Now(), DomainDateTime.Now())
            };
            svc.Setup(s => s.GetByTagAsync("Friend")).ReturnsAsync(list);

            var controller = new PhoneBookController(svc.Object);
            var result = await controller.GetByTag("Friend");

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async Task GetAll_should_return_Ok_with_all_entries()
        {
            var svc = new Mock<IPhoneBookService>();
            var list = new List<EntryDto>
            {
                new(Guid.NewGuid(), "Ali", "Rezaei", "0912", "Friend", DomainDateTime.Now(), DomainDateTime.Now()),
                new(Guid.NewGuid(), "Sara", "Karimi", "0935", "Coworker", DomainDateTime.Now(), DomainDateTime.Now())
            };
            svc.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var controller = new PhoneBookController(svc.Object);
            var result = await controller.GetAll();

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async Task Delete_nonexistent_returns_NotFound()
        {
            var svc = new Mock<IPhoneBookService>();
            svc.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            var controller = new PhoneBookController(svc.Object);
            var result = await controller.Delete(Guid.NewGuid());
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_existing_returns_NoContent()
        {
            var svc = new Mock<IPhoneBookService>();
            svc.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var controller = new PhoneBookController(svc.Object);
            var result = await controller.Delete(Guid.NewGuid());

            result.Should().BeOfType<NoContentResult>();
        }

    }
}