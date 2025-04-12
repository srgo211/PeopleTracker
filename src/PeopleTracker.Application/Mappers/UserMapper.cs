using PeopleTracker.Application.DTOs;
using PeopleTracker.Domain.Entities;

namespace PeopleTracker.Application.Mappers;

/// <summary>Преобразование сущности User в DTO</summary>
public static class UserMapper
{
    /// <summary>Преобразовать User в UserShortDto</summary>
    public static UserShortDto ToShortDto(this User user)
    {
        return new UserShortDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Patronymic = user.Patronymic
        };
    }
}
