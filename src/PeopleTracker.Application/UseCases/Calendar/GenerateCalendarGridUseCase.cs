using PeopleTracker.Application.DTOs.Calendar;
using PeopleTracker.Application.Interfaces;
using PeopleTracker.Application.Mappers;
using PeopleTracker.Domain.Entities;

namespace PeopleTracker.Application.UseCases.Calendar;

/// <summary>UseCase: построение календарной таблицы по всем пользователям</summary>
public sealed class GenerateCalendarGridUseCase
{
    private readonly IUserRepository _userRepo;
    private readonly IJournalRepository _journalRepo;

    /// <summary>Конструктор с внедрением репозиториев</summary>
    public GenerateCalendarGridUseCase(IUserRepository userRepo, IJournalRepository journalRepo)
    {
        _userRepo = userRepo;
        _journalRepo = journalRepo;
    }

    /// <summary>Построить таблицу календаря за период по всем пользователям</summary>
    public async Task<CalendarGridDto> ExecuteAsync(DateOnly from, DateOnly to)
    {
        var users = await _userRepo.GetAllAsync();
        var records = await _journalRepo.GetAllAsync();

        var rows = users.Select(user =>
        {
            var userDays = Enumerable.Range(0, (to.DayNumber - from.DayNumber) + 1)
                .Select(offset => from.AddDays(offset))
                .ToDictionary(
                    day => day,
                    day =>
                    {
                        var record = records.FirstOrDefault(r => r.UserId == user.Id && r.Date == day);
                        return record?.Status ?? TrafficStatus.Null;
                    });

            return new UserCalendarRowDto
            {
                User = user.ToShortDto(),
                Days = userDays
            };
        }).ToList();

        return new CalendarGridDto
        {
            From = from,
            To = to,
            Rows = rows
        };
    }
}