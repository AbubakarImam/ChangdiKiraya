using MediatR;

namespace SpaceRent.Application.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(Guid RoleId) : IRequest<bool>;
