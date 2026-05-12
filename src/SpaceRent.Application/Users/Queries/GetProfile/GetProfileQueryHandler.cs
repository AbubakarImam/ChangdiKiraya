using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Users.DTOs;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Users.Queries.GetProfile;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserProfileDto?>
{
    private readonly UserManager<User> _userManager;

    public GetProfileQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserProfileDto?> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) return null;

        return new UserProfileDto(
            user.Id,
            user.Name,
            user.Email ?? string.Empty,
            user.PhoneNumber,
            user.ProfilePictureUrl,
            user.Bio,
            user.Address,
            user.City,
            user.State,
            user.Country,
            user.CreatedAt,
            user.Role
        );
    }
}
