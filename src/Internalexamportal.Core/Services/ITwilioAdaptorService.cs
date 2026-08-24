using System.Threading.Tasks;

namespace Internalexamportal.Core.Services
{
    public interface ITwilioAdaptorService
    {
        Task SendAsync(string code, string phoneNumber);
    }
}