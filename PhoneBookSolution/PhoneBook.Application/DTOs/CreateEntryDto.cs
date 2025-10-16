namespace PhoneBook.Application.DTOs
{
    public record CreateEntryDto(string FirstName, string LastName, string PhoneNumber, string Tag);
}