using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Users.Commands;
using SpaceRent.Application.Users.DTOs;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Users.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Role = request.Role
        };

        var createdUser = await _userRepository.AddAsync(user, cancellationToken);

        return new UserDto(
            createdUser.Id,
            createdUser.Name,
            createdUser.Email,
            createdUser.Role);
    }
}
