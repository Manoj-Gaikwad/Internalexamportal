using AutoMapper;
using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Privilege
{
	//Model
	public class SaveRolePermissionModel : IRequest<SaveRolePermissionResult>
	{
		public string RoleId { get; set; }
		public IEnumerable<PermissionResult> RolePermissions { get; set; }
	}

	public class RolePermissionModel
	{
		public int Id { get; set; }
		public string RoleId { get; set; }
		public int PermissionId { get; set; }
		public int AccessTypeId { get; set; }
	}


	public class SaveRolePermissionResult
	{
		public bool Succeeded { get; set; }
	}

	//Handler
	public class SaveRolePermissionHandler : IRequestHandler<SaveRolePermissionModel, SaveRolePermissionResult>
	{
		private readonly IMapper _mapper;
		private readonly InternalExamportalContext _context;

		public SaveRolePermissionHandler(
			IMapper mapper,
			 InternalExamportalContext context
			)
		{
			_mapper = mapper;
			_context = context;
		}

		public async Task<SaveRolePermissionResult> Handle(SaveRolePermissionModel model, CancellationToken cancellationToken)
		{

			var result = new SaveRolePermissionResult();

			//Remove Existing Role Permissions
			var existingRolePermissions = _context.RolePermission.Where(v => v.RoleId == model.RoleId).ToList();
			if (existingRolePermissions.Count > 0)
			{
				_context.RolePermission.RemoveRange(existingRolePermissions);
				await _context.SaveChangesAsync();
			}

			//Add New Role Permissions
			foreach (var item in model.RolePermissions)
			{
				if (item.IsChecked)
				{
                    var rolePermission = new RolePermissionModel
                    {
                        Id = 0,
                        RoleId = model.RoleId,
                        PermissionId = item.Id
                    };

                    var rolePermision = _mapper.Map<RolePermission>(rolePermission);
					_context.RolePermission.Add(rolePermision);
				}
			}

			await _context.SaveChangesAsync();
			result.Succeeded = true;
			return result;
		}

	}
}

