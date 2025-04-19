using PeopleTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleTracker.Application.DTOs.Calendar;

/// <summary>Модель строки календаря для одного пользователя</summary>
public sealed class UserCalendarRowDto
{
    /// <summary>Информация о пользователе</summary>
    public required UserShortDto User { get; init; }

    /// <summary>Словарь: дата → статус посещаемости</summary>
    public required Dictionary<DateOnly, TrafficStatus> Days { get; init; }
   
}