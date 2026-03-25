using System.Net;
using System.Net.Mail;

namespace VacationMode.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IConfiguration config, ILogger<SmtpEmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var smtpSection = _config.GetSection("Smtp");
        var host = smtpSection["Host"] ?? "smtp.gmail.com";
        var port = int.Parse(smtpSection["Port"] ?? "587");
        var user = smtpSection["Username"] ?? "";
        var pass = smtpSection["Password"] ?? "";
        var from = smtpSection["From"] ?? user;

        if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
        {
            _logger.LogWarning("SMTP credentials are not configured. Skipping email to {Email}.", toEmail);
            return;
        }

        try
        {
            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(from, "VacationMode"),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            message.To.Add(toEmail);

            await client.SendMailAsync(message);
            _logger.LogInformation("Email sent to {Email} — Subject: {Subject}", toEmail, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
        }
    }
}
