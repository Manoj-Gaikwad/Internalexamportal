using Internalexamportal.Core.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internalexamportal.Core.Services
{
    public class ForgotPasswordLinkUrlService : IForgotPasswordLinkUrlService
    {
        private readonly ForgotPasswordLinkConfiguration _forgotPasswordConfiguration;
        private readonly IHttpContextAccessor _contextAccessor;

        public ForgotPasswordLinkUrlService(
           IOptions<ForgotPasswordLinkConfiguration> forgotPasswordConfiguration,
           IHttpContextAccessor contextAccessor
            )
        {
            _forgotPasswordConfiguration = forgotPasswordConfiguration.Value;
            _contextAccessor = contextAccessor;
        }
        public string GetForgotPasswordUrl(string userId, string code, string BaseUrl)
        {
            return string.Format(BaseUrl, userId, code);
        }
    }
}
