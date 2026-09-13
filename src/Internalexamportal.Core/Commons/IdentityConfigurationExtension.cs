using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Internalexamportal.Core.Configurations;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Internalexamportal.Core.Commons
{
    public static class IdentityConfigurationExtension
    {
        /// <summary>
        /// Add Identity with our custom configuration
        /// </summary>
        /// <param name="services">service collection</param>
        public static void AddIdentityCustom(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            // Get connection string from configuration
            var connectionString = serviceProvider
                    .GetService<IOptions<ConnectionStringsConfiguration>>()
                    .Value.DefaultConnection;

            // Add Identity via asp.net service container
            // Autofac uses this service collection down the 
            // pipeline to register Identity services/objects
            // https://stackoverflow.com/questions/35947598/register-usermanager-with-autofac-in-asp-net-core

            services.AddDbContext<InternalExamportalContext>(options =>
                options.UseNpgsql(connectionString));

            // see http://pioneercode.com/post/authentication-in-a-asp-dot-net-core-api-part-1-identity-access-denied
            // to learn about authentication setup with identity
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<InternalExamportalContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // configure password requirements for identity
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                //Configure lockout settings
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
            });

            // Get token configuration 
            var tokenConfig = serviceProvider.GetService<IOptions<TokenConfiguration>>().Value;

            services.AddAuthentication(
            options =>
            {
                options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme       = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme       = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme      = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme             = JwtBearerDefaults.AuthenticationScheme;
            }
            )
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(tokenConfig.Key)),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = tokenConfig.SiteUrl,
                    ValidateAudience = true,
                    ValidIssuer = tokenConfig.SiteUrl,
                    ValidateIssuer = true,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
            });


            services.AddTransient<IPrincipal>(sp =>
            {
                var contextAccessor = sp.GetService<IHttpContextAccessor>();
                return contextAccessor?.HttpContext?.User ?? new ClaimsPrincipal();
            });

        }

    }
}
