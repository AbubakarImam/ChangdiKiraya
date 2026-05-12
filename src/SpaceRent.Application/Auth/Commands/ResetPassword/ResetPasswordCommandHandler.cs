using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Auth.Models;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, AuthResult>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AuthResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new AuthResult { Success = false, Errors = new List<string> { "Invalid request." } };
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

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
