using Internalexamportal.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Internalexamportal.Web.StartupExtensions
{
    public static class EntityFrameworkExtensions
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<InternalExamportalContext>(options =>
                         options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")))
                         .AddScoped<DbContext, InternalExamportalContext>();
            //.AddDatabaseDeveloperPageExceptionFilter();
        }
    }
}
