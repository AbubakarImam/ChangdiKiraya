using MediatR;
using SpaceRent.Application.Auth.Models;

namespace SpaceRent.Application.Auth.Commands.VerifyEmail;

public record VerifyEmailCommand(string Email, string Token) : IRequest<AuthResult>;
