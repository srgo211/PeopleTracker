using PeopleTracker.Application.DTOs;

namespace PeopleTracker.MauiApp.ViewModels;

public sealed class UserCalendarRowUiVm
{
    public UserShortDto User { get; init; } = default!;
    public ObservableCollection<CalendarCellVm> Cells { get; init; } = new();
}
