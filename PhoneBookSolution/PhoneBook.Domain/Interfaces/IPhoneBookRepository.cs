using PhoneBook.Domain.Entities;
namespace PhoneBook.Domain.Interfaces
{
    public interface IPhoneBookRepository
    {
        Task AddAsync(PhoneBookEntry entry);
        Task UpdateAsync(PhoneBookEntry entry);
        Task<bool> DeleteAsync(Guid id);
        Task<PhoneBookEntry?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<PhoneBookEntry>> GetAllAsync();
        Task<IReadOnlyList<PhoneBookEntry>> GetByTagAsync(string tag);
    }
}