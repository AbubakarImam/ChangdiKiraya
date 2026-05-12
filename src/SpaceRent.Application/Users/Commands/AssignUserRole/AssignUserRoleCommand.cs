using MediatR;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Users.Commands.AssignUserRole;

public record AssignUserRoleCommand(Guid UserId, UserRole Role) : IRequest<bool>;
