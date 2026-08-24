using FluentValidation.AspNetCore;
using Internalexamportal.Core.Features.CreateUser;
using Internalexamportal.Web.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Internalexamportal.Web.StartupExtensions
{
    public static class MvcExtensions
    {
        /// <summary>
        /// Add mvc with custom configuration to <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">service collection</param>
        /// <returns>Mvc builder</returns>
        public static IMvcBuilder AddMvcCustom(this IServiceCollection services)
        {
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            return services.AddTransient<JsonSerializerSettings>(sp =>
            {
                var jsonOption = sp.GetRequiredService<IOptions<MvcNewtonsoftJsonOptions>>();
                return jsonOption.Value.SerializerSettings;
            })
                .AddMvc(options =>
            {
                // get authorization policy
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                // enforce authorize by default. opt-out when necessary.
                options.Filters.Add(new AuthorizeFilter(policy));

                // auto vaidate model state before hitting controller.
                options.Filters.Add(new ValidateModelState());

                // Uncomment this if you need to enforce anti forgery token XSRF.
                // Only needed when we use cookie based authentication
                // options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

            }).AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ContractResolver =
                        new CamelCasePropertyNamesContractResolver();
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddControllersAsServices()
            .AddFluentValidation(fv =>
                    fv.RegisterValidatorsFromAssemblyContaining<CreateUserModelValidator>());

           // .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly<CreateUserModelValidator>());
        }
    }
}
