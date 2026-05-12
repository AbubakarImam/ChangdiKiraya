namespace SpaceRent.Application.Reviews.DTOs;

public record ReviewDto(
    Guid Id,
    Guid SpaceId,
    Guid UserId,
    int Rating,
    string Comment,
    DateTime CreatedAt,
    string? UserName,
    string? UserProfilePictureUrl);
