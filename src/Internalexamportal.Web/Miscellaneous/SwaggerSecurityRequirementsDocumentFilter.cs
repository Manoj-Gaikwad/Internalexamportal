using System;
using System.Collections.Generic;
using Aspose.Pdf.Forms;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Internalexamportal.Web.Miscellaneous
{
    public static class SwaggerExtensions
    {
        public static void UseSwaggerUICustom(this IApplicationBuilder builder)
        {
            builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fusionstak API V1");
            });
        }
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Fusionstak",
                    Description = "Fusionstak sample application.",
                    Contact = new OpenApiContact()
                    {
                        Name = "Fusionstak LLC",
                        Email = "Fusionstak@testemail.com",
                        Url = new Uri("https://Fusionstak.com/")
                    }
                });
                // Add jwt authorization support in swagger ui.                
                // https://stackoverflow.com/a/49035476
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Description = "Authorization header",
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