using MediatR;
using SpaceRent.Application.Auth.Models;

namespace SpaceRent.Application.Auth.Commands.GoogleLogin;

public record GoogleLoginCommand(string IdToken) : IRequest<AuthResult>;
