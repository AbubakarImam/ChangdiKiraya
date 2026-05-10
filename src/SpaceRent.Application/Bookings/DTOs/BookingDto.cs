using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Bookings.DTOs;

public record BookingDto(
    Guid Id,
    Guid SpaceId,
    Guid UserId,
    DateTime StartTime,
    DateTime EndTime,
    BookingStatus Status);
