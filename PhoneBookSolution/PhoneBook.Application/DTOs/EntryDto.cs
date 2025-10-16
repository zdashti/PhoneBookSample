namespace PhoneBook.Application.DTOs
{
    public record EntryDto(Guid Id, string FirstName, string LastName, string PhoneNumber, string Tag, DateTime CreatedAt, DateTime UpdatedAt);
}