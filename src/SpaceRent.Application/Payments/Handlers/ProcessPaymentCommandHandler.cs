using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Payments.Commands;
using SpaceRent.Application.Payments.DTOs;
using SpaceRent.Domain.Entities;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Payments.Handlers;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, PaymentDto>
{
    private readonly IPaymentRepository _paymentRepository;

    public ProcessPaymentCommandHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<PaymentDto> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new Payment
        {
            BookingId = request.BookingId,
            Amount = request.Amount,
            Status = PaymentStatus.Pending
        };

        var processedPayment = await _paymentRepository.AddAsync(payment, cancellationToken);

        return new PaymentDto(
            processedPayment.Id,
            processedPayment.BookingId,
            processedPayment.Amount,
            processedPayment.Status);
    }
}
