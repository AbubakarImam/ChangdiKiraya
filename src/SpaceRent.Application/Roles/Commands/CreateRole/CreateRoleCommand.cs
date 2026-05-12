using MediatR;
using SpaceRent.Application.Roles.DTOs;

namespace SpaceRent.Application.Roles.Commands.CreateRole;

public record CreateRoleCommand(string Name, string? Description, string? Permissions) : IRequest<RoleDto?>;
