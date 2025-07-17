using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace DreamDay.Services
{
    public class FakeEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Just return completed task (no real email sending)
            return Task.CompletedTask;
        }
    }
}
