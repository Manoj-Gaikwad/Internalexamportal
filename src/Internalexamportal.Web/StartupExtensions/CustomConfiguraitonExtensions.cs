using Internalexamportal.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Internalexamportal.Web.StartupExtensions
{
    public static class CustomConfigurationExtension
    {
        public static void AddCustomConfigurations(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // For more details visit
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration
            services.Configure<TokenConfiguration>
                (configuration.GetSection("TokenConfiguration"));
            services.Configure<ConnectionStringsConfiguration>
                (configuration.GetSection("ConnectionStrings"));
            services.Configure<LoggingConfiguration>
                (configuration.GetSection("Logging"));
            services.Configure<ErrorEmailConfiguration>
                (configuration.GetSection("ErrorEmail"));
            services.Configure<ConfirmationEmailConfiguration>
                (configuration.GetSection("ConfirmationEmail"));
            services.Configure<AuthenticationSettingsConfiguration>
               (configuration.GetSection("AuthenticationSettings"));
            services.Configure<ProjectInfoConfiguration>
               (configuration.GetSection("ProjectInfo"));
            services.Configure <TwilioInfoConfiguration>
               (configuration.GetSection("TwilioInfoConfiguration"));
            services.Configure<InitialHookConfiguration>
                (configuration.GetSection("Hook"));
            services.Configure<DefaultUserConfiguration>
               (configuration.GetSection("DefaultUserConfiguration"));
            services.Configure<FileConfiguration>
                (configuration.GetSection("FileConfiguration"));

        }
    }
}
