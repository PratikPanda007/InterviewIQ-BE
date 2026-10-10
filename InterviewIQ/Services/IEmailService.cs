namespace InterviewIQ.Services;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string recipientEmail, string recipientName);
    Task SendPasswordResetEmailAsync(string recipientEmail, string recipientName, string resetLink);
}