using System;
using System.Collections.Generic;
using System.Text;

namespace Internalexamportal.Core.Services
{
    public interface IForgotPasswordLinkUrlService
    {
        string GetForgotPasswordUrl(string userId, string code, string BaseUrl);
    }
}
