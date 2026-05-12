using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Users.DTOs;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UserProfileDto?>
{
    private readonly UserManager<User> _userManager;

    public UpdateProfileCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserProfileDto?> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) return null;

        user.Name = request.Name;
        user.PhoneNumber = request.PhoneNumber;
        user.Bio = request.Bio;
        user.Address = request.Address;
        user.City = request.City;
        user.State = request.State;
        user.Country = request.Country;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception("Failed to update user profile");
        }

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
