using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Internalexamportal.Web.StartupExtensions
{
    public static class SpaStaticFilesCustom
    {
        public static void AddSpaStaticFilesCustom(this IServiceCollection services)
        {
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });
        }

        // Updated method to work with WebApplication
        public static void UseSpaCustom(this WebApplication app)
        {
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (app.Environment.IsDevelopment())
                    spa.UseAngularCliServer(npmScript: "start");
            });
        }
    }
}