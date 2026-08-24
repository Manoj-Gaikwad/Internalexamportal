using Internalexamportal.Core.Configurations;
using Internalexamportal.Core.Features.Admin.testManagement;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Services
{
    public interface IEmailSenderService
    {
        Task SendConfirmationEmail(string email, string message);
        Task SendPasswordEmail(string email, string message);
        Task SendForgotPasswordLink(string email, string message);
        Task SendExamLink(string email, string message,string url);
        Task SendExamResult(SendExamResultEmailModel model,string url);
    }
}