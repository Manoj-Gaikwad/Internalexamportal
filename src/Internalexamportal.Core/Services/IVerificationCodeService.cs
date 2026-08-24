using Internalexamportal.DataAccessLayer.Entities;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Services
{
    public interface IVerificationCodeService
    {
        Task<string> GenerateVerificationCode(User user);
    }
}