using SpaceRent.Domain.Enums;

namespace SpaceRent.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SpaceId { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    // Navigation Properties
    public Space Space { get; set; } = null!;
    public User User { get; set; } = null!;
    public Payment? Payment { get; set; }
}
