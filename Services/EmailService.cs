using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace bkpDN.Services;

public class EmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string message)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_smtpSettings.Name, _smtpSettings.Email));
        email.To.Add(new MailboxAddress("", recipientEmail));
        email.Subject = subject;

        email.Body = new TextPart("plain")
        {
            Text = message
        };

        using (var smtp = new SmtpClient())
        {
            smtp.Connect(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_smtpSettings.Email, _smtpSettings.Pass);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}

public class SmtpSettings
{
    public string Server { get; set; }
    public int Port { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Pass { get; set; }
}
