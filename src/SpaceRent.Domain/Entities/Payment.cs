using SpaceRent.Domain.Enums;

namespace SpaceRent.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    
    // Navigation Property
    public Booking Booking { get; set; } = null!;
}
