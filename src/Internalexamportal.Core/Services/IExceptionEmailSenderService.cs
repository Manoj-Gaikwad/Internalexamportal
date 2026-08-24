using Internalexamportal.Core.Configurations;
using System;
using System.Security.Principal;

namespace Internalexamportal.Core.Services
{
    public interface IExceptionEmailSenderService
    {
        void SendEmail(Exception exception, 
            ErrorEmailConfiguration errorEmailConfiguration, 
            IPrincipal principal);
    }
}
