using Microsoft.AspNetCore.Builder;

namespace Internalexamportal.Web.StartupExtensions
{
    public static class HandleExceptionErrorMiddlewareExtension
    {
        public static IApplicationBuilder UseHandleExceptionErrorMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HandleExceptionErrorMiddleware>();
        }
    }
}
