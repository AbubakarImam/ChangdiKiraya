using MediatR;
using SpaceRent.Application.Roles.DTOs;

namespace SpaceRent.Application.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid RoleId) : IRequest<RoleDto?>;
