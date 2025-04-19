namespace PeopleTracker.Application.DTOs.Calendar;

/// <summary>Отображение всей таблицы календаря по всем пользователям</summary>
public sealed class CalendarGridDto
{
    /// <summary>Дата начала периода</summary>
    public DateOnly From { get; init; }

    /// <summary>Дата окончания периода</summary>
    public DateOnly To { get; init; }

    /// <summary>Список строк, по одному пользователю каждая</summary>
    public required IReadOnlyList<UserCalendarRowDto> Rows { get; init; }

}
