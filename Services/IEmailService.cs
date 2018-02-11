using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Atut.Services
{
    public interface IEmailService : IIdentityMessageService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}