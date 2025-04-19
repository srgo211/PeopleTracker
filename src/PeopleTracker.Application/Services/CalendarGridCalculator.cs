using PeopleTracker.Application.DTOs.Calendar;
using PeopleTracker.Domain.Entities;

namespace PeopleTracker.Application.Services;


// <summary>Сервис агрегации данных таблицы календаря</summary>
public static class CalendarGridCalculator
{
    /// <summary>Получить количество статусов для одного пользователя</summary>
    public static Dictionary<TrafficStatus, int> GetStatusCounts(UserCalendarRowDto row)
    {
        return Enum
            .GetValues<TrafficStatus>()
            .ToDictionary(
                status => status,
                status => row.Days.Values.Count(v => v == status)
            );
    }

    /// <summary>Получить количество статусов по всем пользователям в разрезе дней</summary>
    public static Dictionary<DateOnly, Dictionary<TrafficStatus, int>> GetDailyTotals(IEnumerable<UserCalendarRowDto> rows)
    {
        return rows
            .SelectMany(row => row.Days)
            .GroupBy(pair => pair.Key) // группируем по дате
            .ToDictionary(
                group => group.Key, // дата
                group => Enum
                    .GetValues<TrafficStatus>()
                    .ToDictionary(
                        status => status,
                        status => group.Count(p => p.Value == status)
                    )
            );
    }

    /// <summary>Получить количество определенного статуса по каждому дню</summary>
    public static Dictionary<DateOnly, int> GetStatusTotalsByDay(IEnumerable<UserCalendarRowDto> rows, TrafficStatus status)
    {
        return rows
            .SelectMany(row => row.Days)
            .Where(x => x.Value == status)
            .GroupBy(x => x.Key)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}