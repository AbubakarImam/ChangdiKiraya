using MediatR;
using SpaceRent.Application.Payments.DTOs;

namespace SpaceRent.Application.Payments.Commands;

public record ProcessPaymentCommand(
    Guid BookingId,
    decimal Amount) : IRequest<PaymentDto>;
