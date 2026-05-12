using MediatR;
using SpaceRent.Application.Auth.Models;

namespace SpaceRent.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<AuthResult>;
