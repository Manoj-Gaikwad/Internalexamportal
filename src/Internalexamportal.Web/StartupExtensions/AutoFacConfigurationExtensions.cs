//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Security.Principal;
//using Autofac;
//using Autofac.Extensions.DependencyInjection;
//using Internalexamportal.Core;
//using Internalexamportal.Core.Commons;
//using Internalexamportal.Core.Features.CreateUser;
//using Internalexamportal.Core.FileSystem;
//using Internalexamportal.Core.Services;
//using Internalexamportal.DataAccessLayer;
//using Internalexamportal.DataAccessLayer.Contracts;
//using Internalexamportal.DataAccessLayer.Entities;
//using InternalExamportal.DataAccessLayer;
//using InternalExamportal.DataAccessLayer.Contracts;
//using InternalExamportal.DataAccessLayer.EntityConfiguration;
//using MediatR;
//using MediatR.Pipeline;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;

//namespace Internalexamportal.Web.StartupExtensions
//{

//    public static class AutoFacConfigurationExtensions
//    {
//        /// <summary>
//        /// Configures AutoFac for Dependency Injection.
//        /// For more info visit http://docs.autofac.org/en/latest/integration/aspnetcore.html#quick-start
//        /// </summary>
//        /// <param name="services">service collection.</param>
//        /// <returns>container</returns>
//        public static IContainer AddAutoFac(this IServiceCollection services)
//        {
//            // Create the container builder.
//            var builder = new ContainerBuilder();

//            // configure mediatr with AutoFac
//            //builder.RegisterMediatr();

//            // configure Automapper with AutoFac
//            builder.RegisterAutoMapper();
//            builder.RegisterEntityConfigurator();

//            // Register dependencies, populate the services from
//            // the collection, and build the container. If you want
//            // to dispose of the container at the end of the app,
//            // be sure to keep a reference to it as a property or field.
//            // builder.RegisterType<SampleService>().As<ISampleService>();
//
//          -- builder.RegisterType<InternalExamportalContext>().As<DbContext>();

//           - builder.RegisterGeneric(typeof(Repository<>))
//                .As(typeof(IRepository<>)).InstancePerDependency();

//           - builder.RegisterGeneric(typeof(DeleteRepository<>))
//               .As(typeof(IDeleteRepository<>)).InstancePerDependency();

//           -- builder.RegisterType<ExceptionErrorEmailService>()
//               .As<IExceptionEmailSenderService>();

//           -- builder.RegisterType<DatabaseExceptionLogService>()
//                .As<IExceptionLogService>();

//           -- builder.RegisterType<EmailSenderService>()
//                .As<IEmailSenderService>();

//            //builder.RegisterType<SubjectRepository>()
//            //  .As<ISubjectRepository>();

//           -- builder.RegisterType<GetRolePermissionService>()
//                .As<IGetRolePermissionService>();

//           -- builder.RegisterType<GenerateEmailConfirmationUrlService>()
//                .As<IGenerateEmailConfirmationUrlService>();

//           -- builder.RegisterType<JwtSecurityTokenGenerationService>()
//                .As<IJwtSecurityTokenGenerationService>();

//           -- builder.RegisterType<ClaimsGenerationService>()
//                .As<IClaimsGenerationService>();

//           -- builder.RegisterType<TwilioAdaptorService>()
//                .As<ITwilioAdaptorService>();

//           -- builder.RegisterType<SmsNotificationService>()
//                .As<ISmsNotificationService>();

//           -- builder.RegisterType<VerificationCodeService>()
//                .As<IVerificationCodeService>();

//           -- builder.RegisterType<DocumentService>()
//                .As<IDocumentService>();

//           -- builder.RegisterType<DocumentStorageService>()
//                .As<IDocumentStorage>();

//           -- builder.RegisterType<FluentLocal>()
//                .As<IFluentLocal>();

//           -- builder.RegisterType<LocalFileSystem>()
//                .As<IFileSystem>();

//           -- builder.RegisterType<ClientManagerService>()
//                .As<IClientManagerService>();

//           -- builder.RegisterType<RollNumberingService>()
//                .As<IRollNumberingService>();


//            //  builder.RegisterType<SecuritySettings>()
//            //.As<SecuritySettings>();

//           -- builder.RegisterType<DefaultUserCreatorService>()
//               .As<IDefaultUserCreatorService>();

//            builder.Populate(services);

//            // this allows to inject user claims principal outside mvc controllers
//          --  builder.Register(ctx => ctx.Resolve<IHttpContextAccessor>().HttpContext?.User ?? new System.Security.Claims.ClaimsPrincipal())
//                .As<IPrincipal>()
//                .InstancePerDependency();

//          --  builder.RegisterType<ForgotPasswordLinkUrlService>()
//                .As<IForgotPasswordLinkUrlService>();

//           -- builder.RegisterType<PasswordChangeLogService>()
//               .As<IPasswordChangeLogService>();

//            // build container and return
//            return builder.Build();
//        }

//        private static void RegisterEntityConfigurator(this ContainerBuilder builder)
//        {
//            // filter to identify types that implement IEntityTypeConfiguration<TEntity>
//            bool entityConfigurationFilter(Type t) => t.GetInterfaces()
//                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

//           // get all types that implement IEntityTypeConfiguration<TEntity>

//           var entityConfigurationTypes = Assembly
//                   .GetAssembly(typeof(ApplicationErrorConfiguration))
//                   .GetTypes()
//                   .Where(entityConfigurationFilter)
//                   .ToArray();

//            // register all IEntityTypeConfiguration<TEntity>
//            builder.RegisterTypes(entityConfigurationTypes).AsSelf();

//            // register EntityConfigurator as singleton.
//          --  builder.Register(ctx => {
//                var context = ctx.Resolve<IComponentContext>();
//                var configs = entityConfigurationTypes.Select(type => context.Resolve(type));
//                return new EntityConfigurator(configs);
//            })
//            .As<IEntityConfigurator>()
//            .SingleInstance();
//        }

//           /// <summary>
//        /// Configures AutoMapper with AutoFac
//        /// For more info visit http://dotnetthoughts.net/using-automapper-in-aspnet-core-project/
//        /// </summary>
//        /// <param name="builder">Autofac Container Builder</param>
//        private static void RegisterAutoMapper(this ContainerBuilder builder)
//        {
//            // create mapper configuration
//            var config = new AutoMapper.MapperConfiguration(cfg =>
//            {
//                cfg.AddProfiles(new List<Type>
//                {
//                    typeof(InternalExamportalCoreMapperProfile),
//                    //.. add more profiles here
//                });
//            });

//            // user configuration to create mapper instance
//            var mapper = config.CreateMapper();

//            // register instance as signleton with AutoFac
//            builder.RegisterInstance(mapper).SingleInstance();
//        }

//        /// <summary>
//        /// Configures Mediatr Library with AutoFac
//        /// For more info visit https://github.com/jbogard/MediatR/wiki#autofac
//        /// </summary>
//        /// <param name="builder">Autofac Container Builder</param>
//        private static void RegisterMediatr(this ContainerBuilder builder)
//        {
//            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
//                .AsImplementedInterfaces();

//            var mediatrOpenTypes = new[]
//            {
//                typeof(IRequestHandler<,>),
//                typeof(INotificationHandler<>),
//            };

//            foreach (var mediatrOpenType in mediatrOpenTypes)
//            {
//                builder
//                    .RegisterAssemblyTypes(typeof(CreateUserHandler).GetTypeInfo().Assembly)
//                    .AsClosedTypesOf(mediatrOpenType)
//                    .AsImplementedInterfaces();
//            }


//            // It appears Autofac returns the last registered types first
//            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
//            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

//            builder.Register<ServiceFactory>(ctx =>
//            {
//                var c = ctx.Resolve<IComponentContext>();
//                return t => c.Resolve(t);
//            });
//        }
//    }
//}