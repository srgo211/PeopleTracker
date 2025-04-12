using PeopleTracker.Domain.Entities;

namespace PeopleTracker.Application.DTOs;

/// <summary>Отчет по пользователю за указанный период</summary>
public sealed class UserJournalSummaryDto
{
    /// <summary>Id пользователя</summary>
    public int UserId { get; init; }

    /// <summary>ФИО пользователя (может быть null)</summary>
    public UserShortDto User { get; init; }

    /// <summary>Дата начала периода</summary>
    public DateOnly From { get; init; }

    /// <summary>Дата окончания периода</summary>
    public DateOnly To { get; init; }

    /// <summary>Количество записей по каждому статусу</summary>
    public required Dictionary<TrafficStatus, int> StatusCounts { get; init; }

    /// <summary>Подробные записи пользователя</summary>
    public required IReadOnlyList<UserJournalRecordDto> Records { get; init; }

    /// <summary>Общее число активных дней</summary>
    public int TotalDays { get; init; }
}
