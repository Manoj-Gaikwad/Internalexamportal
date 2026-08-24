using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Privilege
{
	public class UpdateRoleStatusModel : IRequest<bool>
	{
		public string RoleId { get; set; }
	}

	public class UpdateRoleStatusHandler : IRequestHandler<UpdateRoleStatusModel, bool>
	{
		private readonly RoleManager<Role> _roleManager;
		public UpdateRoleStatusHandler(RoleManager<Role> roleManager)
		{
			_roleManager = roleManager;
		}

		public async Task<bool> Handle(UpdateRoleStatusModel request, CancellationToken cancellationToken)
		{
			var role = await _roleManager.FindByIdAsync(request.RoleId);
			role.IsActive = !role.IsActive;
			await _roleManager.UpdateAsync(role);
			return true;
		}
	}
}
