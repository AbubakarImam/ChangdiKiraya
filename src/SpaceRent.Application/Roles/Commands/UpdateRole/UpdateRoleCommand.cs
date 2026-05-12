using MediatR;
using SpaceRent.Application.Roles.DTOs;

namespace SpaceRent.Application.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(Guid RoleId, string Name, string? Description, string? Permissions) : IRequest<RoleDto?>;
