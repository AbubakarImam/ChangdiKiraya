using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Users.DTOs;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserProfileDto?>
{
    private readonly UserManager<User> _userManager;

    public GetUserByIdQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserProfileDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) return null;

        // Note: For public profiles, we usually omit sensitive info like Email or PhoneNumber,
        // but since we reuse UserProfileDto, we will just map it as is.
        // In a real prod app, a separate PublicUserDto is better.
        return new UserProfileDto(
            user.Id,
            user.Name,
            string.Empty, // Hide email for public profile
            null, // Hide phone number
            user.ProfilePictureUrl,
            user.Bio,
            null, // Hide exact address
            user.City,
            user.State,
            user.Country,
            user.CreatedAt,
            user.Role
        );
    }
}
