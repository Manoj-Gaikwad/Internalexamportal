using Internalexamportal.Core.Commons;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using Internalexamportal.DataAccessLayer.Entities;
using System;
using System.Security.Principal;

namespace Internalexamportal.Core.Services
{
    public class DatabaseExceptionLogService : IExceptionLogService
    {
        public readonly InternalExamportalContext _context;

        public DatabaseExceptionLogService(InternalExamportalContext context)
        {
            _context = context;
        }
        public void LogException(Exception exception, IPrincipal principal)
        {
            //Get user id of current user.
            var userId = principal.Identity.GetUserId();

            ApplicationError applicationError = new ApplicationError()
            {
                Exception = exception.Message,
                Message = exception.StackTrace,
                TimeStamp = DateTime.Now,
                User = !String.IsNullOrEmpty(userId) ? userId : null
            };

            _context.ApplicationError.Add(applicationError);
            _context.SaveChanges();
        }

    }
}
