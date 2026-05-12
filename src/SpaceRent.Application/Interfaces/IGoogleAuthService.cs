namespace SpaceRent.Application.Interfaces;

public record GoogleTokenPayload(string Email, string Name, string GivenName, string FamilyName, string Picture);

public interface IGoogleAuthService
{
    Task<GoogleTokenPayload?> ValidateTokenAsync(string idToken);
}
