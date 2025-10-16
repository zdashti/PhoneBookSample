using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Interfaces;
using System.Collections.Concurrent;

namespace PhoneBook.Infrastructure.Repositories
{
    public class InMemoryPhoneBookRepository : IPhoneBookRepository
    {
        private readonly ConcurrentDictionary<Guid, PhoneBookEntry> _store = new();

        public Task AddAsync(PhoneBookEntry entry)
        {
            if (!_store.TryAdd(entry.Id, entry))
                throw new InvalidOperationException("Could not add entry (duplicate id)");
            return Task.CompletedTask;
        }

        public Task UpdateAsync(PhoneBookEntry entry)
        {
            _store[entry.Id] = entry;
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            return Task.FromResult(_store.TryRemove(id, out _));
        }

        public Task<PhoneBookEntry?> GetByIdAsync(Guid id)
        {
            _store.TryGetValue(id, out var entry);
            return Task.FromResult(entry);
        }

        public Task<IReadOnlyList<PhoneBookEntry>> GetAllAsync()
        {
            var list = _store.Values.OrderBy(x => x.Name.ToString()).ToList();
            return Task.FromResult((IReadOnlyList<PhoneBookEntry>)list);
        }

        public Task<IReadOnlyList<PhoneBookEntry>> GetByTagAsync(string tag)
        {
            var list = _store.Values
                .Where(x => string.Equals(x.Tag.Value, tag, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.Name.ToString())
                .ToList();
            return Task.FromResult((IReadOnlyList<PhoneBookEntry>)list);
        }
    }
}