using Microsoft.AspNetCore.Identity;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
