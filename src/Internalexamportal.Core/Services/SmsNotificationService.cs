using Internalexamportal.Core.Configurations;
using Internalexamportal.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Services
{
    public class SmsNotificationService : ISmsNotificationService
    {
        private readonly ITwilioAdaptorService _twilioAdapterService;
        private readonly ProjectInfoConfiguration _projectInfo;
        private readonly UserManager<User> _userManager;
        public SmsNotificationService(
            ITwilioAdaptorService twilioAdapterService,
            IOptions<ProjectInfoConfiguration> projectInfo,
            UserManager<User> userManager

            )
        {
            _twilioAdapterService = twilioAdapterService;
            _projectInfo = projectInfo.Value;
            _userManager = userManager;
        }
        public  async Task SendAuthenticationCodeAsync(string phoneNumber, string code)
        {
            //Register token provider in usermanager to generate Code for sms. 
            _userManager.RegisterTokenProvider("Phone", new PhoneNumberTokenProvider<User> { });

            //Generate text message string.
            string message = $"Your " +
                $"{_projectInfo.ProjectName} verification code is " +
                $"{code}";

            //Call Twilio Service to send message
            await _twilioAdapterService.SendAsync(phoneNumber, message);
        }
    }
}
