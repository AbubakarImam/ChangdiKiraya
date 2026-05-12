using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Roles.DTOs;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto?>
{
    private readonly RoleManager<Role> _roleManager;

    public CreateRoleCommandHandler(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<RoleDto?> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        if (await _roleManager.RoleExistsAsync(request.Name))
            throw new Exception($"Role {request.Name} already exists.");

        var role = new Role
        {
            Name = request.Name,
            Description = request.Description,
            Permissions = request.Permissions
        };

        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
            throw new Exception("Failed to create role");

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
