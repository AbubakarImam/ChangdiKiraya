using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SpaceRent.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    [MaxLength(500)]
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation property for users with this role
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    
    // Permissions could be added here as a JSON string or separate table
    public string? Permissions { get; set; } // JSON array of permission strings
}