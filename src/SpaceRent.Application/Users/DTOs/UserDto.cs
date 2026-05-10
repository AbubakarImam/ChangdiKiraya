using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Users.DTOs;

public record UserDto(
    Guid Id,
    string Name,
    string Email,
    UserRole Role);
