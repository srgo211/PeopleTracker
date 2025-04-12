using PeopleTracker.Application.Interfaces;
using PeopleTracker.Domain.Entities;

namespace PeopleTracker.Infrastructure.InMemory;

/// <summary>InMemory реализация репозитория пользователей</summary>
public sealed class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    private readonly object _lock = new();

    /// <summary>Получить всех пользователей</summary>
    public Task<IReadOnlyList<User>> GetAllAsync()
    {
        lock (_lock)
        {
            return Task.FromResult<IReadOnlyList<User>>(_users.ToList());
        }
    }

    /// <summary>Получить пользователя по Id</summary>
    public Task<User?> GetByIdAsync(int userId)
    {
        lock (_lock)
        {
            return Task.FromResult(_users.FirstOrDefault(x => x.Id == userId));
        }
    }

    /// <summary>Добавить нового пользователя</summary>
    public Task AddAsync(User user)
    {
        lock (_lock)
        {
            if (_users.Any(x => x.Id == user.Id))
                throw new InvalidOperationException("Пользователь с таким Id уже существует");

            _users.Add(user);
        }
        return Task.CompletedTask;
    }

    /// <summary>Обновить существующего пользователя</summary>
    public Task UpdateAsync(User user)
    {
        lock (_lock)
        {
            var index = _users.FindIndex(x => x.Id == user.Id);
            if (index >= 0)
            {
                _users[index] = user;
            }
        }
        return Task.CompletedTask;
    }

    /// <summary>Удалить пользователя по Id</summary>
    public Task<bool> DeleteAsync(int userId)
    {
        lock (_lock)
        {
            var removed = _users.RemoveAll(x => x.Id == userId);
            return Task.FromResult(removed > 0);
        }
    }
}