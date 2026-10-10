using InterviewIQ.Models.Configurations;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace InterviewIQ.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly FrontendSettings _frontendSettings;

    public EmailService(IOptions<EmailSettings> settings, IOptions<FrontendSettings> frontendSettings)
    {
        _settings = settings.Value;
        _frontendSettings = frontendSettings.Value;
    }

    public async Task SendWelcomeEmailAsync(string recipientEmail, string recipientName)
    {
        var message = new MimeMessage();

        message.From.Add(
            new MailboxAddress(
                _settings.FromName,
                _settings.FromEmail));

        message.To.Add(
            new MailboxAddress(
                recipientName,
                recipientEmail));

        message.Subject = "Welcome to InterviewIQ";

        message.Body = new TextPart("plain")
        {
            Text =
                $"Hi {recipientName},\n\n" +
                "Welcome to InterviewIQ!\n\n" +
                "Your account has been successfully created. " +
                "You can now start preparing for your interviews with AI-powered mock interviews.\n\n" +
                "Best regards,\n" +
                "InterviewIQ Team"
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _settings.SmtpServer,
            _settings.Port,
            SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            _settings.Username,
            _settings.Password);

        await smtp.SendAsync(message);

        await smtp.DisconnectAsync(true);
    }

    public async Task SendPasswordResetEmailAsync(string recipientEmail, string recipientName, string resetLink)
    {
        var message = new MimeMessage();

        message.From.Add(
            new MailboxAddress(
                _settings.FromName,
                _settings.FromEmail));

        message.To.Add(
            new MailboxAddress(
                recipientName,
                recipientEmail));

        message.Subject = "Reset your InterviewIQ password";

        var safeName = System.Net.WebUtility.HtmlEncode(recipientName);
        var safeLink = System.Net.WebUtility.HtmlEncode(resetLink);

        message.Body = new BodyBuilder
        {
            TextBody =
                $"Hi {recipientName},\n\n" +
                "We received a request to reset your InterviewIQ password.\n\n" +
                $"Use this link within 10 minutes:\n{resetLink}\n\n" +
                "If you did not request this reset, you can ignore this email.\n\n" +
                "InterviewIQ Team",

            HtmlBody =
                $"<p>Hi {safeName},</p>" +
                "<p>We received a request to reset your InterviewIQ password.</p>" +
                $"<p><a href=\"{safeLink}\">Reset your password</a></p>" +
                "<p>This link expires in 10 minutes and can only be used once.</p>" +
                "<p>If you did not request this reset, you can ignore this email.</p>" +
                "<p>InterviewIQ Team</p>"
        }.ToMessageBody();

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _settings.SmtpServer,
            _settings.Port,
            SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            _settings.Username,
            _settings.Password);

        await smtp.SendAsync(message);

        await smtp.DisconnectAsync(true);
    }
}