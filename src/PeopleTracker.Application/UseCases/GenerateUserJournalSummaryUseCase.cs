using PeopleTracker.Application.DTOs;
using PeopleTracker.Application.Interfaces;
using PeopleTracker.Application.Mappers;
using PeopleTracker.Domain.Entities;

namespace PeopleTracker.Application.UseCases;

/// <summary>UseCase: Получить статистику по пользователю за период</summary>
public sealed class GenerateUserJournalSummaryUseCase
{
    private readonly IUserRepository _userRepo;
    private readonly IJournalRepository _journalRepo;

    /// <summary>Конструктор с внедрением зависимостей</summary>
    public GenerateUserJournalSummaryUseCase(IUserRepository userRepo, IJournalRepository journalRepo)
    {
        _userRepo = userRepo;
        _journalRepo = journalRepo;
    }

    /// <summary>Получить отчет по конкретному пользователю за указанный период</summary>
    public async Task<UserJournalSummaryDto> ExecuteAsync(int userId, DateOnly from, DateOnly to)
    {
        var user = await _userRepo.GetByIdAsync(userId)
                   ?? throw new InvalidOperationException("Пользователь не найден");

        var recordsInPeriod = await _journalRepo.GetByUserAndPeriodAsync(userId, from, to);

        var statusCounts = Enum.GetValues<TrafficStatus>()
            .ToDictionary(
                status => status,
                status => recordsInPeriod.Count(r => r.Status == status)
            );

        var recordDtos = recordsInPeriod
            .Select(r => new UserJournalRecordDto
            {
                Date = r.Date,
                Status = r.Status
            })
            .ToList();

        var total = statusCounts
            .Where(kv => kv.Key != TrafficStatus.Null)
            .Sum(kv => kv.Value);

        return new UserJournalSummaryDto
        {
            User = user.ToShortDto(),
            UserId = user.Id,
            From = from,
            To = to,
            StatusCounts = statusCounts,
            Records = recordDtos,
            TotalDays = total
        };
    }
}