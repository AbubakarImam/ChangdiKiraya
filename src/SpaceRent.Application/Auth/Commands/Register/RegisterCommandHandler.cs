using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Auth.Models;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResult { Success = false, Errors = new List<string> { "Email already in use." } };
        }

        var user = new User
        {
            Email = request.Email,
            UserName = request.Email, // Identity requires UserName, we use Email
            Name = request.Name,
            Role = UserRole.User
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Success = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // 7 days refresh token validity
        await _userManager.UpdateAsync(user);

        return new AuthResult
        {
            Success = true,
            Token = token,
            RefreshToken = refreshToken
        };
    }
}
