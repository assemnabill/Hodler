namespace Hodler.Domain.Shared.EmailService
{
    public interface IEmailService
    {
        public Task SendEmailAsync(List<string> toEmails, string subject, string body, CancellationToken cancellationToken, bool isHtml = false);
        public Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken, bool isHtml = false);
    }
}
