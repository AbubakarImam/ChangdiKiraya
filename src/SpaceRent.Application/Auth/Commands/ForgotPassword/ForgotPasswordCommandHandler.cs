using System.Web;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(UserManager<User> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return true; // Return true even if user doesn't exist to prevent email enumeration
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(token);
        
        var resetLink = $"{request.ClientUri}?email={HttpUtility.UrlEncode(request.Email)}&token={encodedToken}";

        var body = $"<p>Please reset your password by clicking <a href='{resetLink}'>here</a>.</p>";
        await _emailService.SendEmailAsync(request.Email, "Reset Password", body);

        return true;
    }
}
