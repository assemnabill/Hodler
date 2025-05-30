using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Hodler.Domain.Shared.EmailService
{
    public class MailKitEmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<MailKitEmailService> _logger;

        public MailKitEmailService(IOptions<EmailSettings> emailSettings, ILogger<MailKitEmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }
        public async Task SendEmailAsync(List<string> toEmails, string subject, string body, CancellationToken cancellationToken, bool isHtml = false)
        {
            try
            {
                var message = new MimeMessage();
                message.Date = DateTime.Now;
                message.From.Add(new MailboxAddress
                (
                    address: _emailSettings.SenderEmail,
                    name: _emailSettings.SenderName
                ));
                foreach (var email in toEmails)
                {
                    message.To.Add(new MailboxAddress(string.Empty, email));
                }
                message.Subject = subject;
                message.Body = isHtml ?
                              new TextPart(MimeKit.Text.TextFormat.Html) { Text = body } :
                              new TextPart(MimeKit.Text.TextFormat.Plain) { Text = body };
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync
                    (
                        host: _emailSettings.MailServer,
                        port: _emailSettings.MailPort,
                        useSsl: _emailSettings.EnableSsl,
                        cancellationToken
                    );

                    await client.AuthenticateAsync
                    (
                        _emailSettings.SenderEmail,
                        _emailSettings.Password,
                        cancellationToken
                    );

                    await client.SendAsync(message, cancellationToken);
                    await client.DisconnectAsync(true, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken, bool isHtml = false)
        {
            await SendEmailAsync(new List<string> { toEmail }, subject, body, cancellationToken, isHtml);
        }
    }
}
