using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Services
{
    public class ClientManagerService : IClientManagerService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly InternalExamportalContext _context;
        private readonly IPrincipal _principle;
        public ClientManagerService(
            IMapper mapper,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            InternalExamportalContext context,
             IPrincipal principle
        )
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _principle = principle;
        }

        public async Task<Tuple<int, bool>> GetClientId()
        {
            var loggedInUserId = _principle.Identity.GetUserId();

            var user = await _userManager.FindByIdAsync(loggedInUserId);

            IList<string> userRole = await _userManager.GetRolesAsync(user);
            if (userRole.Count > 0)
            {
                var userRoleName = userRole[0];
                if (userRoleName == GlobalConstants.SuperAdminRoleName)
                {
                    return new Tuple<int, bool>(user.ClientId, true);
                }
                else
                {
                    return new Tuple<int, bool>(user.ClientId, false);
                }
            }

            return new Tuple<int, bool>(-1, false);
            
        }

    }
}
