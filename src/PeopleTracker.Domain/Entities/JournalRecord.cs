namespace PeopleTracker.Domain.Entities;


/// <summary> </summary>
public sealed class JournalRecord
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public DateOnly Date { get; init; }
    public TrafficStatus Status { get; init; }
}
