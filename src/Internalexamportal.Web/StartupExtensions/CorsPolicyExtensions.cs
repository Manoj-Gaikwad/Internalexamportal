using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors;
using Microsoft.Extensions.DependencyInjection;

namespace Internalexamportal.Web.StartupExtensions
{
	public static class CorsPolicyExtensions
	{
		/// <summary>
		///	Initialize CORS here.
		///	Allow access to different web origins based on the application's environment.
		/// </summary>
		/// <param name="services">service collection</param>
		/// <param name="env">current hosting environment</param>
		public static void AddCorsPolicyCustom(this IServiceCollection services)
		{
		    var env = services.BuildServiceProvider().GetService<IHostingEnvironment>();

            if (env.IsDevelopment())
			{
				services.AddCors(options =>
				{
					options.AddPolicy("FusionstakCorsPolicy",
						builder => builder.AllowAnyOrigin()
						.AllowAnyMethod()
						// Enable application to accept all headers
						// Authentication header is not allowed by default, required for JWT Auth Token.
						// https://docs.microsoft.com/en-us/aspnet/core/security/cors#set-the-exposed-response-headers
						.AllowAnyHeader());
						//.AllowCredentials()
				});
			}
			else
			{
				services.AddCors(options =>
				{
					options.AddPolicy("FusionstakCorsPolicy",
						builder => builder.WithOrigins("www.fusionstak.com")  // specify the origin url here
						.AllowAnyHeader());
				});
			}

			// add global CORS policy
			//services.Configure<MvcOptions>(options =>
			//{
			//	options.Filters.Add(new CorsAuthorizationFilterFactory("FusionstakCorsPolicy"));
			//});
		}
	}
}
