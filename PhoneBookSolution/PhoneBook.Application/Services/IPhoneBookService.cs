using PhoneBook.Application.DTOs;

namespace PhoneBook.Application.Services
{
    public interface IPhoneBookService
    {
        Task<EntryDto> AddAsync(CreateEntryDto dto);
        Task<EntryDto?> UpdateAsync(UpdateEntryDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IReadOnlyList<EntryDto>> GetByTagAsync(string tag);
        Task<IReadOnlyList<EntryDto>> GetAllAsync();
        Task<EntryDto?> GetByIdAsync(Guid id);
    }
}