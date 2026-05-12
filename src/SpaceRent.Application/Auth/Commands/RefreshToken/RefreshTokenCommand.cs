using MediatR;
using SpaceRent.Application.Auth.Models;

namespace SpaceRent.Application.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string Token, string RefreshToken) : IRequest<AuthResult>;
