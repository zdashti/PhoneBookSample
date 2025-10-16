using PhoneBook.Application.DTOs;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Interfaces;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Application.Services
{
    public class PhoneBookService : IPhoneBookService
    {
        private readonly IPhoneBookRepository _repo;

        public PhoneBookService(IPhoneBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<EntryDto> AddAsync(CreateEntryDto dto)
        {
            var name = new PersonName(dto.FirstName, dto.LastName);
            var phone = new PhoneNumber(dto.PhoneNumber);
            var tag = new Tag(dto.Tag);

            var entry = PhoneBookEntry.Create(name, phone, tag);
            await _repo.AddAsync(entry);

            return Map(entry);
        }

        public async Task<EntryDto?> UpdateAsync(UpdateEntryDto dto)
        {
            var existing = await _repo.GetByIdAsync(dto.Id);
            if (existing is null) return null;

            if (!string.IsNullOrWhiteSpace(dto.FirstName) || !string.IsNullOrWhiteSpace(dto.LastName))
            {
                var fn = dto.FirstName ?? existing.Name.FirstName;
                var ln = dto.LastName ?? existing.Name.LastName;
                existing.UpdateName(new PersonName(fn, ln));
            }

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                existing.UpdatePhoneNumber(new PhoneNumber(dto.PhoneNumber));

            if (!string.IsNullOrWhiteSpace(dto.Tag))
                existing.UpdateTag(new Tag(dto.Tag));

            await _repo.UpdateAsync(existing);
            return Map(existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<IReadOnlyList<EntryDto>> GetByTagAsync(string tag)
        {
            var list = await _repo.GetByTagAsync(tag);
            return list.Select(Map).ToList();
        }

        public async Task<IReadOnlyList<EntryDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(Map).ToList();
        }

        public async Task<EntryDto?> GetByIdAsync(Guid id)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e is null) return null;
            return Map(e);
        }

        private static EntryDto Map(PhoneBookEntry e) =>
            new EntryDto(e.Id, e.Name.FirstName, e.Name.LastName, e.PhoneNumber.Value, e.Tag.Value, e.CreatedAt, e.UpdatedAt);
    }
}
