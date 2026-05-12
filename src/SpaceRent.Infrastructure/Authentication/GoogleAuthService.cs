using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using SpaceRent.Application.Interfaces;

namespace SpaceRent.Infrastructure.Authentication;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly ILogger<GoogleAuthService> _logger;

    public GoogleAuthService(ILogger<GoogleAuthService> logger)
    {
        _logger = logger;
    }

    public async Task<GoogleTokenPayload?> ValidateTokenAsync(string idToken)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            return new GoogleTokenPayload(
                payload.Email,
                payload.Name,
                payload.GivenName,
                payload.FamilyName,
                payload.Picture
            );
        }
        catch (InvalidJwtException ex)
        {
            _logger.LogError(ex, "Invalid Google ID token");
            return null;
        }
    }
}
