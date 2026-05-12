using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Users.Commands.UploadProfilePicture;

public class UploadProfilePictureCommandHandler : IRequestHandler<UploadProfilePictureCommand, string?>
{
    private readonly UserManager<User> _userManager;
    private readonly IFileStorageService _fileStorageService;

    public UploadProfilePictureCommandHandler(UserManager<User> userManager, IFileStorageService fileStorageService)
    {
        _userManager = userManager;
        _fileStorageService = fileStorageService;
    }

    public async Task<string?> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) return null;

        var url = await _fileStorageService.SaveAsync(request.FileStream, request.FileName, request.ContentType, "profile-pictures", cancellationToken);
        
        // Remove old picture if exists and not default (not implementing deletion of old file for simplicity, but could)
        user.ProfilePictureUrl = url;
        user.UpdatedAt = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return url;
    }
}
