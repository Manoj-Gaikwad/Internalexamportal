using Internalexamportal.Web.Miscellaneous;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Internalexamportal.Web.StartupExtensions
{
    public static class SwaggerExtensions
    {
        public static void UseSwaggerUICustom(
            this IApplicationBuilder builder)
        {
            builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fusionstak API V1");
            });
        }

        public static void AddSwagger(this IServiceCollection services) {
            services.AddSwaggerGen(option =>
            {
                option.CustomSchemaIds(type => type.FullName);
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Api Endpoints",
                    Description = "Stack Application For Fusionstak",

                    Contact = new OpenApiContact()
                    {
                        Name = "Fusionstak LLC", 
                        Email = "fusionstak@testemail.com", 
                        Url = new System.Uri("https://fusionstak.com/")
                    }
                });


                // Add jwt authorization support in swagger ui.
                // https://stackoverflow.com/a/49035476
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme                        {
                            Reference = new OpenApiReference                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"                            }
                        },
                        new[] { "readAccess", "writeAccess" }
                    }
                });
            });

        }
    }
}
