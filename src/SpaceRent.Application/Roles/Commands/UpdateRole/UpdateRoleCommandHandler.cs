using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Roles.DTOs;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Roles.Commands.UpdateRole;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto?>
{
    private readonly RoleManager<Role> _roleManager;

    public UpdateRoleCommandHandler(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<RoleDto?> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
        if (role == null) return null;

        role.Name = request.Name;
        role.Description = request.Description;
        role.Permissions = request.Permissions;
        role.UpdatedAt = DateTime.UtcNow;

        var result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded)
            throw new Exception("Failed to update role");

        return new RoleDto(
            role.Id,
            role.Name,
            role.Description,
            role.Permissions,
            role.CreatedAt,
            role.UpdatedAt
        );
    }
}
