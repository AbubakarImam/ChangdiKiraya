namespace SpaceRent.Application.Roles.DTOs;

public record RoleDto(
    Guid Id,
    string Name,
    string? Description,
    string? Permissions,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
