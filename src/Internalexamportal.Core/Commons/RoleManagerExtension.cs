using Internalexamportal.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Commons
{
    public static class RoleManagerExtension
    {
        public static async Task TryCreateRole<TRole>(
            this RoleManager<TRole> _roleManager,
            string roleName)
            where TRole : Role, new()
        {
            // check if there is a role with the name in database.
            var role = await _roleManager.FindByNameAsync(roleName);

            // do not proceed role is already in database
            if (role != null) return;

            // create a new role with that name
            role = new TRole { Name = roleName };

            // add new role to database.
            await _roleManager.CreateAsync(role);
        }

        public static async Task SeedRoles<TRole>(this RoleManager<TRole> _roleManager)
            where TRole : Role, new()
        {
            //Create roles if they do not exist
            //Second parameter is RoleName for either updating the name of Role OR 
            //empty for deleting the existing Role
            await _roleManager.TryCreateRole(GlobalConstants.SuperAdminRoleName);
            await _roleManager.TryCreateRole(GlobalConstants.AdminRoleName);
            await _roleManager.TryCreateRole(GlobalConstants.CandidateRoleName);
        }

        public static async Task CreateDefaultRole<TRole>(this RoleManager<TRole> _roleManager)
           where TRole : Role, new()
        {
            //Create a default role if it doesnt exist
            //Second parameter is RoleName for either updating the name of Role OR 
            //empty for deleting the existing Role
            await _roleManager.TryCreateRole(GlobalConstants.SuperAdminRoleName);
        }

    }
}
