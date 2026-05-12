using MediatR;
using SpaceRent.Application.Auth.Models;

namespace SpaceRent.Application.Auth.Commands.Register;

public record RegisterCommand(string Name, string Email, string Password) : IRequest<AuthResult>;
