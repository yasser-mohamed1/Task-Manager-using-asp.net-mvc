using Microsoft.AspNetCore.Identity.UI.Services;

internal class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        throw new NotImplementedException();
    }
}