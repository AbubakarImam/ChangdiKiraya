using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Auth.Models;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Auth.Commands.GoogleLogin;

public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, AuthResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IGoogleAuthService _googleAuthService;

    public GoogleLoginCommandHandler(
        UserManager<User> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        IGoogleAuthService googleAuthService)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _googleAuthService = googleAuthService;
    }

    public async Task<AuthResult> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
    {
        var payload = await _googleAuthService.ValidateTokenAsync(request.IdToken);
        if (payload == null)
        {
            return new AuthResult { Success = false, Errors = new List<string> { "Invalid Google token." } };
        }

        var user = await _userManager.FindByEmailAsync(payload.Email);

        if (user == null)
        {
            user = new User
            {
                Email = payload.Email,
                UserName = payload.Email,
                Name = payload.Name ?? "Google User",
                Role = UserRole.Customer,
                EmailConfirmed = true // Trusted from Google
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return new AuthResult
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new AuthResult
        {
            Success = true,
            Token = token,
            RefreshToken = refreshToken
        };
    }
}
