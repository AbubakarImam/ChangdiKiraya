using MediatR;
using SpaceRent.Application.Roles.DTOs;

namespace SpaceRent.Application.Roles.Commands.UpdateRolePermissions;

public record UpdateRolePermissionsCommand(Guid RoleId, string Permissions) : IRequest<RoleDto?>;
