using System.Web;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Auth.Commands.ResendVerification;

public class ResendVerificationCommandHandler : IRequestHandler<ResendVerificationCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public ResendVerificationCommandHandler(UserManager<User> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<bool> Handle(ResendVerificationCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || user.EmailConfirmed)
        {
            return true; // Pretend it worked to avoid email enumeration
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(token);
        
        var verifyLink = $"{request.ClientUri}?email={HttpUtility.UrlEncode(request.Email)}&token={encodedToken}";

        var body = $"<p>Please verify your email by clicking <a href='{verifyLink}'>here</a>.</p>";
        await _emailService.SendEmailAsync(request.Email, "Verify Email", body);

        return true;
    }
}
