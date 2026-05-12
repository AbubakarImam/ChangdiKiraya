using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Roles.DTOs;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly RoleManager<Role> _roleManager;

    public GetRoleByIdQueryHandler(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<RoleDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
        if (role == null) return null;

        return new RoleDto(
            role.Id,
            role.Name ?? string.Empty,
            role.Description,
            role.Permissions,
            role.CreatedAt,
            role.UpdatedAt
        );
    }
}
