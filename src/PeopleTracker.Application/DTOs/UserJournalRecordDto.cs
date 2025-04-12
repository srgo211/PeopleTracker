using PeopleTracker.Domain.Entities;

namespace PeopleTracker.Application.DTOs;

/// <summary>Запись посещаемости пользователя по дате</summary>
public sealed class UserJournalRecordDto
{
    /// <summary>Дата посещения</summary>
    public DateOnly Date { get; init; }

    /// <summary>Статус в этот день</summary>
    public TrafficStatus Status { get; init; }
}
