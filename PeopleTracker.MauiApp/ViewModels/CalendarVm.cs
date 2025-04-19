

using PeopleTracker.Domain.Entities;

namespace PeopleTracker.MauiApp.ViewModels;

internal class CalendarVm : ViewModel
{
    private readonly GenerateCalendarGridUseCase _useCase;

    public ObservableCollection<UserCalendarRowUiVm> UiRows { get; } = new();

    public ObservableCollection<DateOnly> Dates { get; private set; } = new();

    public CalendarVm(GenerateCalendarGridUseCase useCase)
    {
        _useCase = useCase;
    }

    public async Task LoadAsync(DateOnly from, DateOnly to)
    {
        var result = await _useCase.ExecuteAsync(from, to);

        Dates.Clear();
        foreach (var d in Enumerable.Range(0, to.DayNumber - from.DayNumber + 1)
                     .Select(offset => from.AddDays(offset)))
            Dates.Add(d);

        UiRows.Clear();

        foreach (var row in result.Rows)
        {
            var cells = Dates.Select(date => new CalendarCellVm
            {
                Date = date,
                Status = row.Days.TryGetValue(date, out var s) ? s : TrafficStatus.Null
            });

            UiRows.Add(new UserCalendarRowUiVm
            {
                User = row.User,
                Cells = new ObservableCollection<CalendarCellVm>(cells)
            });
        }
    }
}
