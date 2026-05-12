using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resend;
using SpaceRent.Application.Interfaces;

namespace SpaceRent.Infrastructure.Email;

public class ResendEmailService : IEmailService
{
    private readonly ResendSettings _settings;
    private readonly ILogger<ResendEmailService> _logger;
    private readonly IResend _resend;

    public ResendEmailService(IResend resend, IOptions<ResendSettings> settings, ILogger<ResendEmailService> logger)
    {
        _resend = resend;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var message = new EmailMessage
            {
                From = $"{_settings.FromName} <{_settings.FromEmail}>",
                To = { to },
                Subject = subject,
                HtmlBody = body
            };

            await _resend.EmailSendAsync(message);
            _logger.LogInformation("Email sent successfully to {To}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", to);
        }
    }
}
