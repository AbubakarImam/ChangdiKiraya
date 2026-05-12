using MediatR;

namespace SpaceRent.Application.Auth.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email, string ClientUri) : IRequest<bool>;
