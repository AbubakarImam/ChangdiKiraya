using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Auth.Models;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RefreshTokenCommandHandler(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        // Ensure token has proper format
        if (!tokenHandler.CanReadToken(request.Token))
        {
            return new AuthResult { Success = false, Errors = new List<string> { "Invalid token format." } };
        }

        var jwtToken = tokenHandler.ReadJwtToken(request.Token);
        var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

        if (string.IsNullOrEmpty(emailClaim))
        {
            return new AuthResult { Success = false, Errors = new List<string> { "Invalid token claims." } };
        }

        var user = await _userManager.FindByEmailAsync(emailClaim);

        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return new AuthResult { Success = false, Errors = new List<string> { "Invalid or expired refresh token." } };
        }

        var newToken = _jwtTokenGenerator.GenerateToken(user);
        var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new AuthResult
        {
            Success = true,
            Token = newToken,
            RefreshToken = newRefreshToken
        };
    }
}
