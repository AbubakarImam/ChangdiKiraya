namespace SpaceRent.Infrastructure.Email;

public class ResendSettings
{
    public const string SectionName = "Resend";
    public string ApiKey { get; set; } = null!;
    public string FromEmail { get; set; } = null!;
    public string FromName { get; set; } = null!;
}
