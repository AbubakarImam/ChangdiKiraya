using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler : IRequestHandler<AssignUserRoleCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public AssignUserRoleCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) return false;

        var newRoleName = request.Role.ToString();

        // Optional: Remove from all other roles if a user can only have one role
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);

        var result = await _userManager.AddToRoleAsync(user, newRoleName);
        if (result.Succeeded)
        {
            user.Role = request.Role; // Update the enum property as well
            await _userManager.UpdateAsync(user);
        }

        return result.Succeeded;
    }
}
