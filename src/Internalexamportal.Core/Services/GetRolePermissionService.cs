using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Services
{
	public class GetRolePermissionService : IGetRolePermissionService
	{
		private readonly IMapper _mapper;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly InternalExamportalContext _context;
		public GetRolePermissionService(
			IMapper mapper,
			UserManager<User> userManager,
			RoleManager<Role> roleManager,
			InternalExamportalContext context
		)
		{
			_mapper = mapper;
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
		}

		public async Task<IEnumerable<RolePermission>> GetRolePermissions(User user)
		{
			IList<string> userRole = await _userManager.GetRolesAsync(user);
			var rolePermissions = new List<RolePermission>();

			if (userRole.Count > 0)
			{
				var userRoleName = userRole[0];
				var role = await _roleManager.FindByNameAsync(userRoleName);
				rolePermissions = _context.RolePermission.Where(x => x.RoleId == role.Id).ToList();
			}

			var result = _mapper.Map<IEnumerable<RolePermission>>(rolePermissions);

			return result;
		}

	}
}
