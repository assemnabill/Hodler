namespace Hodler.Domain.Shared.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(List<string> toEmails, string subject, string body, CancellationToken cancellationToken, bool isHtml = false);
        Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken, bool isHtml = false);
    }
}
