using PeopleTracker.Domain.Entities;

namespace PeopleTracker.Application.Interfaces;

/// <summary>Интерфейс репозитория для журнала посещаемости</summary>
public interface IJournalRepository
{
    /// <summary>Получить все записи</summary>
    Task<IReadOnlyList<JournalRecord>> GetAllAsync();

    /// <summary>Получить запись по Id</summary>
    Task<JournalRecord?> GetByIdAsync(int id);

    /// <summary>Получить записи по пользователю и дате</summary>
    Task<JournalRecord?> GetByUserAndDateAsync(int userId, DateOnly date);

    /// <summary>Добавить запись</summary>
    Task AddAsync(JournalRecord record);

    /// <summary>Обновить запись</summary>
    Task UpdateAsync(JournalRecord record);

    /// <summary>Удалить запись по Id</summary>
    Task<bool> DeleteAsync(int id);
}
