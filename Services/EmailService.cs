using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config["EmailSettings:From"]));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = body
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_config["EmailSettings:Host"], int.Parse(_config["EmailSettings:Port"]), false);
        await smtp.AuthenticateAsync(
            _config["EmailSettings:From"],
            _config["EmailSettings:Password"]
        );
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
