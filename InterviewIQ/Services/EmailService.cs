using InterviewIQ.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace InterviewIQ.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
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
}