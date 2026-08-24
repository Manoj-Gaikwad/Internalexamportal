using Internalexamportal.Core.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net;

namespace Internalexamportal.Core.Services
{
    public class GenerateEmailConfirmationUrlService : IGenerateEmailConfirmationUrlService
    {
        private readonly ConfirmationEmailConfiguration _emailConfiguration;
        private readonly IHttpContextAccessor _contextAccessor;
        public GenerateEmailConfirmationUrlService(
           IOptions<ConfirmationEmailConfiguration> emailConfiguration,
           IHttpContextAccessor contextAccessor
        )
        {
            _emailConfiguration = emailConfiguration.Value;
            _contextAccessor = contextAccessor;
        }

        public string GetEmailConfirmationUrl(string userId, string emailConfirmationCode)
        {

            //encode the generated code to preserve characters while re-routig
            var encodedCode = WebUtility.UrlEncode(emailConfirmationCode);

            var baseUrl = $"{_contextAccessor.HttpContext.Request.Scheme}://" +
                $"{_contextAccessor.HttpContext.Request.Host.ToUriComponent()}";

            //Return formatted string to be used as a call back url. Editable in appsetting.json. 
            return string.Format(_emailConfiguration.ConfirmationEmailUrl, 
                baseUrl, userId, encodedCode);
        }
    }
}

