using PeopleTracker.Domain.Entities;

namespace PeopleTracker.MauiApp.ViewModels;

public sealed class CalendarCellVm : ViewModel
{
    #region Date
    private DateOnly _Date;
    public DateOnly Date
    {
        get => _Date;
        set => Set(ref _Date, value);
    }
    #endregion
    #region Status
    private TrafficStatus _Status;

    public TrafficStatus Status
    {
        get => _Status;
        set => Set(ref _Status, value);
    }
    #endregion
    public string Symbol => Status switch
    {
        TrafficStatus.Present => "✔",
        TrafficStatus.Absent => "❌",
        TrafficStatus.Sick => "🤒",
        TrafficStatus.Dismissed => "🚫",
        TrafficStatus.DayOff => "📅",
        _ => ""
    };
}