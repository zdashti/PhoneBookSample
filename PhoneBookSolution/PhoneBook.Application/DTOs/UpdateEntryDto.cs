namespace PhoneBook.Application.DTOs
{
    public record UpdateEntryDto(Guid Id, string? FirstName, string? LastName, string? PhoneNumber, string? Tag);
}