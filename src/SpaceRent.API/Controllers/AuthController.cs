using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceRent.Application.Auth.Commands.ChangePassword;
using SpaceRent.Application.Auth.Commands.ForgotPassword;
using SpaceRent.Application.Auth.Commands.GoogleLogin;
using SpaceRent.Application.Auth.Commands.Login;
using SpaceRent.Application.Auth.Commands.Logout;
using SpaceRent.Application.Auth.Commands.RefreshToken;
using SpaceRent.Application.Auth.Commands.Register;
using SpaceRent.Application.Auth.Commands.ResendVerification;
using SpaceRent.Application.Auth.Commands.ResetPassword;
using SpaceRent.Application.Auth.Commands.VerifyEmail;
using SpaceRent.Application.Auth.Queries.GetMe;

namespace SpaceRent.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success) return BadRequest(result.Errors);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success) return Unauthorized(result.Errors);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success) return Unauthorized(result.Errors);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdString = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var success = await _mediator.Send(new LogoutCommand(userId));
        return success ? Ok() : BadRequest();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "If the email is valid, a reset link has been sent." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success) return BadRequest(result.Errors);
        return Ok(new { message = "Password has been reset successfully." });
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success) return BadRequest(result.Errors);
        return Ok(new { message = "Email verified successfully." });
    }

    [HttpPost("resend-verification")]
    public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "If the email is valid, a verification link has been sent." });
    }

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success) return Unauthorized(result.Errors);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userIdString = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var command = new ChangePasswordCommand(userId, request.CurrentPassword, request.NewPassword);
        var result = await _mediator.Send(command);
        if (!result.Success) return BadRequest(result.Errors);
        return Ok(new { message = "Password changed successfully." });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userIdString = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var user = await _mediator.Send(new GetMeQuery(userId));
        if (user == null) return NotFound();

        return Ok(new
        {
            user.Id,
            user.Name,
            user.Email,
            user.Role
        });
    }
}

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
