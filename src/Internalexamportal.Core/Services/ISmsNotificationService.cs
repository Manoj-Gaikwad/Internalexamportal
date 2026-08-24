using System.Threading.Tasks;

namespace Internalexamportal.Core.Services
{
    public interface ISmsNotificationService
    {
        Task SendAuthenticationCodeAsync(string phoneNumber, string code);
       // Task SendForgotPasswordLink(string phoneNumber, object callbackUrl);
    }
}