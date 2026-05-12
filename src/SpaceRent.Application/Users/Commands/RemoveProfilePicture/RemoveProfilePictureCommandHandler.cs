using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Users.Commands.RemoveProfilePicture;

public class RemoveProfilePictureCommandHandler : IRequestHandler<RemoveProfilePictureCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IFileStorageService _fileStorageService;

    public RemoveProfilePictureCommandHandler(UserManager<User> userManager, IFileStorageService fileStorageService)
    {
        _userManager = userManager;
        _fileStorageService = fileStorageService;
    }

    public async Task<bool> Handle(RemoveProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null || string.IsNullOrEmpty(user.ProfilePictureUrl)) return false;

        // Optionally delete the file from storage
        try
        {
            await _fileStorageService.DeleteAsync(user.ProfilePictureUrl, cancellationToken);
        }
        catch
        {
            // Ignore if file is already deleted or not found
        }

        user.ProfilePictureUrl = null;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}
