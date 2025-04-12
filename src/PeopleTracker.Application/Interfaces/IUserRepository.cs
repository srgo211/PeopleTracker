using PeopleTracker.Domain.Entities;

namespace PeopleTracker.Application.Interfaces;

/// <summary>Интерфейс репозитория для работы с пользователями</summary>
public interface IUserRepository
{
    /// <summary>Получить всех пользователей</summary>
    Task<IReadOnlyList<User>> GetAllAsync();

    /// <summary>Получить пользователя по Id</summary>
    Task<User?> GetByIdAsync(int userId);

    /// <summary>Добавить нового пользователя</summary>
    Task AddAsync(User user);

    /// <summary>Обновить пользователя</summary>
    Task UpdateAsync(User user);

    /// <summary>Удалить пользователя по Id</summary>
    Task<bool> DeleteAsync(int userId);
}

