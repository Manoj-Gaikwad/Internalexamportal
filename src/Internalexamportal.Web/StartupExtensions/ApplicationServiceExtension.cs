using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.FileSystem;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Internalexamportal.Web.StartupExtensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var configurationTypes = typeof(EntityConfigurator).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                .ToArray();

            var configurations = new List<object>();
            foreach (var type in configurationTypes)
            {
                var ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                if (ctor != null) configurations.Add(ctor.Invoke(null));
            }

            return services
                 .AddSingleton<IEnumerable<object>>(configurations)
                 .AddTransient<IEntityConfigurator, EntityConfigurator>()
                 .AddTransient<IExceptionEmailSenderService, ExceptionErrorEmailService>()
                 .AddTransient<IEmailSenderService, EmailSenderService>()
                 .AddTransient<IGenerateEmailConfirmationUrlService, GenerateEmailConfirmationUrlService>()
                 .AddTransient<IClaimsGenerationService, ClaimsGenerationService>()
                 .AddTransient<IDocumentService, DocumentService>()
                 .AddTransient<ITwilioAdaptorService, TwilioAdaptorService>()
                 .AddTransient<ISmsNotificationService, SmsNotificationService>()
                 .AddTransient<IPasswordChangeLogService, PasswordChangeLogService>()
                 .AddTransient<IDefaultUserCreatorService, DefaultUserCreatorService>()
                 .AddTransient<IExceptionLogService, DatabaseExceptionLogService>()
                 .AddTransient<IFluentLocal, FluentLocal>()
                 .AddTransient<IFileSystem, LocalFileSystem>()
                 .AddTransient<IGetRolePermissionService, GetRolePermissionService>()
                 .AddTransient<IJwtSecurityTokenGenerationService, JwtSecurityTokenGenerationService>()
                 .AddTransient<IVerificationCodeService, VerificationCodeService>()
                 .AddTransient<IDocumentStorage, DocumentStorageService>()
                 .AddTransient<IClientManagerService, ClientManagerService>()
                 .AddTransient<IRollNumberingService, RollNumberingService>()
                 .AddTransient<IForgotPasswordLinkUrlService, ForgotPasswordLinkUrlService>()
                 .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                 .AddScoped(typeof(IDeleteRepository<>), typeof(DeleteRepository<>));

            
             //.AddTransient<ITokenFactory, TokenFactory>()
            // .AddTransient<IPrivilegeservice, Privilegeservice>()
            //.Configure<RefreshTokenHasherOptions>(options => { })
            //.AddTransient<IRefreshTokenHasher, RefreshTokenHasher>()
            //.AddSingleton<IHostedservice, CleanRefreshTokenHostedService>()
            //.AddTransient<ijwttokenmanager, jwttokenmanager>();
        }
    }
}





