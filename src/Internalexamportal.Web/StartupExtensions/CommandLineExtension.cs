using System;
using System.Linq;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Configurations;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;

namespace Internalexamportal.Web.StartupExtensions
{
    public static class CommandLineExtension
    {
        /// <summary>
        /// Fluent Command Line handler to process commands.
        /// </summary>
        /// <param name="host">IWebHost</param>
        /// <returns>IWebHost</returns>
        public static IHost UseCommandLine(this IHost host, string[] commands )
        {
            var services = host.Services.GetService<IServiceScopeFactory>();

            //string[] args = host
            //    .Services
            //    .GetService<IOptions<InitialHookConfiguration>>()?
            //    .Value
            //    ._commandList;
            
            if (commands == null || commands.Length == 0) return host;
            
            using (var scope = services.CreateScope())
            {

                // drops database if found.
                //if (args.Contains("drop-database"))
                //{
                //    Console.WriteLine("Droping database....");
                //    scope.ServiceProvider.GetService<EquipmentManagementContext>().Database.EnsureDeleted();
                //}

                //creates database if doesnot exist.
                // and applies all pending migration.
                if (commands.Contains("create-database"))
                {
                    Console.WriteLine("Creating/updating database....");
                    scope.ServiceProvider.GetService<InternalExamportalContext>().Database.Migrate();
                }

                //insert / update / delete seed data into database table.
                if (commands.Contains("seed-database"))
                {
                    Console.WriteLine("Seeding database....");
                    //scope.ServiceProvider.GetService<InternalExamportalContext>().SeedDatabase();
                    scope.ServiceProvider.GetService<IDefaultUserCreatorService>().CreateUser();
                }

                // seed application roles
                if (commands.Contains("seed-roles"))
                {

                    Console.WriteLine("Seeding roles....");
                    scope.ServiceProvider.GetService<RoleManager<Role>>()
                        .SeedRoles().Wait();
                }

                //create default role that has permission to manage other roles.
                if (commands.Contains("create-default-role"))
                {
                    Console.WriteLine("Seeding roles....");
                    scope.ServiceProvider.GetService<RoleManager<Role>>()
                        .CreateDefaultRole().Wait();
                }

                // create deafult user if user email and password is provided in appsettings
                if (commands.Contains("create-default-user"))
                {
                    Console.WriteLine("Creating default user....");
                    scope.ServiceProvider.GetService<IDefaultUserCreatorService>().CreateUser();
                }
            }

            // return and continue application if "stop" parameter is not present
            if (!commands.Contains("stop")) return host;

            // pre terminate application 
            Console.WriteLine("Exiting on stop command");
            Environment.Exit(0);

            return host;
        }
    }
}
