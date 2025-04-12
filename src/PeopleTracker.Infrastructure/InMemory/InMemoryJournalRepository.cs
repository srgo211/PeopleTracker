using PeopleTracker.Application.Interfaces;
using PeopleTracker.Domain.Entities;

namespace PeopleTracker.Infrastructure.InMemory;

/// <summary>InMemory реализация репозитория журнала</summary>
public sealed class InMemoryJournalRepository : IJournalRepository
{
    private readonly List<JournalRecord> _datas = new();
    private readonly object _lock = new();

    /// <summary>Добавить запись</summary>
    public Task AddAsync(JournalRecord record)
    {
        lock (_lock)
        {
            _datas.Add(record);
        }
        return Task.CompletedTask;
    }

    /// <summary>Обновить запись</summary>
    public Task UpdateAsync(JournalRecord record)
    {
        lock (_lock)
        {
            var index = _datas.FindIndex(x => x.Id == record.Id);
            if (index >= 0)
            {
                _datas[index] = record;
            }
        }
        return Task.CompletedTask;
    }

    /// <summary>Удалить запись по Id</summary>
    public Task<bool> DeleteAsync(int id)
    {
        lock (_lock)
        {
            var removed = _datas.RemoveAll(x => x.Id == id);
            return Task.FromResult(removed > 0);
        }
    }

    /// <summary>Получить все записи</summary>
    public Task<IReadOnlyList<JournalRecord>> GetAllAsync()
    {
        lock (_lock)
        {
            return Task.FromResult<IReadOnlyList<JournalRecord>>(_datas.ToList());
        }
    }

    /// <summary>Получить запись по Id</summary>
    public Task<JournalRecord?> GetByIdAsync(int id)
    {
        lock (_lock)
        {
            return Task.FromResult(_datas.FirstOrDefault(x => x.Id == id));
        }
    }

    /// <summary>Получить запись по пользователю и дате</summary>
    public Task<JournalRecord?> GetByUserAndDateAsync(int userId, DateOnly date)
    {
        lock (_lock)
        {
            return Task.FromResult(_datas.FirstOrDefault(x => x.UserId == userId && x.Date == date));
        }
    }
}
