using MediatR;
using SpaceRent.Application.Users.DTOs;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Users.Commands;

public record CreateUserCommand(
    string Name,
    string Email,
    UserRole Role) : IRequest<UserDto>;
