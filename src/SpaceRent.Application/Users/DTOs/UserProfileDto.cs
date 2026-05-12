using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Users.DTOs;

public record UserProfileDto(
    Guid Id,
    string Name,
    string Email,
    string? PhoneNumber,
    string? ProfilePictureUrl,
    string? Bio,
    string? Address,
    string? City,
    string? State,
    string? Country,
    DateTime CreatedAt,
    UserRole Role);
