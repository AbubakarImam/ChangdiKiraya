using MediatR;
using SpaceRent.Application.Users.DTOs;

namespace SpaceRent.Application.Users.Commands.UpdateProfile;

public record UpdateProfileCommand(
    Guid UserId,
    string Name,
    string? PhoneNumber,
    string? Bio,
    string? Address,
    string? City,
    string? State,
    string? Country) : IRequest<UserProfileDto?>;
