using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}
