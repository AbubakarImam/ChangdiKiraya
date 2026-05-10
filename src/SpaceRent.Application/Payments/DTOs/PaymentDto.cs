using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Payments.DTOs;

public record PaymentDto(
    Guid Id,
    Guid BookingId,
    decimal Amount,
    PaymentStatus Status);
