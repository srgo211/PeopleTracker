using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PeopleTracker.Domain.Entities;


/// <summary>
/// Статус посещаемости / движения пользователя в конкретный день
/// </summary>
public enum TrafficStatus
{
    Null,

    /// <summary>Был</summary>
    Present,

    /// <summary>Не был</summary>
    Absent,

    /// <summary>Болел</summary>
    Sick,

    /// <summary>Выбыл (отчислен, ушёл)</summary>
    Dismissed,

    /// <summary>Выходной день</summary>
    DayOff
}

public class User
{
    public int Id { get; set; }
    public string Name { get; init; } 
    public string LastName { get; init; }
    public string Patronymic { get; init; }
    
}
