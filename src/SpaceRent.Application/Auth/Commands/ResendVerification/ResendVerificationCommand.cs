using MediatR;

namespace SpaceRent.Application.Auth.Commands.ResendVerification;

public record ResendVerificationCommand(string Email, string ClientUri) : IRequest<bool>;
