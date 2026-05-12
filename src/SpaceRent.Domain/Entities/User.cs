using Microsoft.AspNetCore.Identity;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    
    // Authentication properties
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Profile properties
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Role properties
    public Guid? RoleId { get; set; }
    public virtual Role? UserRole { get; set; }
    
    // Navigation properties
    public virtual ICollection<Space> Spaces { get; set; } = new List<Space>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
