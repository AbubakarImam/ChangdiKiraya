using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Auth.Models;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Auth.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, AuthResult>
{
    private readonly UserManager<User> _userManager;

    public ChangePasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AuthResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            return new AuthResult { Success = false, Errors = new List<string> { "User not found." } };
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Success = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        return new AuthResult { Success = true };
    }
}
