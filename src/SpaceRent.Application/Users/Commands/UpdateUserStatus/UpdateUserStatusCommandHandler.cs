using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Users.Commands.UpdateUserStatus;

public class UpdateUserStatusCommandHandler : IRequestHandler<UpdateUserStatusCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public UpdateUserStatusCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) return false;

        // If inactive, lock out until year 9999. If active, remove lockout.
        if (!request.IsActive)
        {
            await _userManager.SetLockoutEnabledAsync(user, true);
            var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            return result.Succeeded;
        }
        else
        {
            var result = await _userManager.SetLockoutEndDateAsync(user, null);
            return result.Succeeded;
        }
    }
}
