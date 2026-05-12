using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Roles.DTOs;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Roles.Commands.UpdateRolePermissions;

public class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, RoleDto?>
{
    private readonly RoleManager<Role> _roleManager;

    public UpdateRolePermissionsCommandHandler(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<RoleDto?> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
        if (role == null) return null;

        role.Permissions = request.Permissions;
        role.UpdatedAt = DateTime.UtcNow;

        var result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded)
            throw new Exception("Failed to update role permissions");

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
