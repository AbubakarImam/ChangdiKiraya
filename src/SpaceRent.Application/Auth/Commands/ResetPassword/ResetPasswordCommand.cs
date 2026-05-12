using MediatR;
using SpaceRent.Application.Auth.Models;

namespace SpaceRent.Application.Auth.Commands.ResetPassword;

public record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest<AuthResult>;
