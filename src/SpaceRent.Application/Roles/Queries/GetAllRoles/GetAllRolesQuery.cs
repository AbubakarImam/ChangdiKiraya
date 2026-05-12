using MediatR;
using SpaceRent.Application.Roles.DTOs;

namespace SpaceRent.Application.Roles.Queries.GetAllRoles;

public record GetAllRolesQuery : IRequest<List<RoleDto>>;
