namespace PeopleTracker.Application.DTOs;

/// <summary>Сжатая информация о пользователе</summary>
public sealed class UserShortDto
{
    /// <summary>Уникальный идентификатор</summary>
    public int Id { get; init; }

    /// <summary>Имя</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Фамилия</summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>Отчество</summary>
    public string Patronymic { get; init; } = string.Empty;

    /// <summary>ФИО</summary>
    public string FullName => $"{LastName} {Name} {Patronymic}";
}