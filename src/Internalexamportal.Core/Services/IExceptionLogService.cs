using System;
using System.Security.Principal;

namespace Internalexamportal.Core.Services
{
   public interface IExceptionLogService
    {
        void LogException(Exception exception, IPrincipal principal);
    }
}
