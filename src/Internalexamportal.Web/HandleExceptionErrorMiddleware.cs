using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Configurations;
using Internalexamportal.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Internalexamportal.Web
{
    public class HandleExceptionErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public HandleExceptionErrorMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(
            HttpContext context,
            IPrincipal principal,
            IExceptionLogService exceptionLogService,
            IExceptionEmailSenderService errorEmailSender,
            IOptions<LoggingConfiguration> loggingConfiguration,
            IOptions<ErrorEmailConfiguration> errorEmailConfiguration,
            JsonSerializerSettings jsonSerializerSettings
        )
        {
            try
            {
                await _next(context);
            }
            catch (HttpException httpException)
            {
                await httpException.SetContext(context, jsonSerializerSettings);
            }
                catch (Exception exception)
            {
                //Only log in error in the database if LogExceptionDatabase property
                //(in appSetting.json) is set to true
                if (loggingConfiguration.Value.LogExceptionInDatabase)
                {
                    exceptionLogService.LogException(exception, principal);
                }

                errorEmailSender.SendEmail(exception, errorEmailConfiguration.Value, principal);

                //Throw to response with 500 internal server error
                throw;
            }
        }

    }

    public static class HandleExceptionErrorMiddlewareExtension
    {
        public static IApplicationBuilder UseHandleExceptionErrorMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HandleExceptionErrorMiddleware>();
        }
    }
}

