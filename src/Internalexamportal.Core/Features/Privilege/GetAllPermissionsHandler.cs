using System;
using System.Collections.Generic;
using AutoMapper;
using MediatR;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.Core.Commons;
using Microsoft.EntityFrameworkCore;

namespace Internalexamportal.Core.Features.Privilege
{
    public class GetAllPermissionModel : IRequest<GetAllPermissionResult>
    {
        public string RoleId { get; set; }
    }

    //Result
    public class GetAllPermissionResult
    {
        public IEnumerable<RoleResult> RoleResult { get; set; }
        public List<PermissionResult> RolePermissions { get; set; }
    }

    public class PermissionResult
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string PermissionName { get; set; }
        public bool IsActive { get; set; }

        //RolePermission Properties
        public bool IsChecked { get; set; }
    }

    public class GetAllPermissionHandler : IRequestHandler<GetAllPermissionModel, GetAllPermissionResult>
    {
        private readonly IMapper _mapper;
        private readonly IPrincipal _principal;
        private readonly InternalExamportalContext _context;

        public GetAllPermissionHandler(
            IMapper mapper,
             IPrincipal principal,
             InternalExamportalContext context
            )
        {
            _mapper = mapper;
            _principal = principal;
            _context = context;
        }

        public async Task<GetAllPermissionResult> Handle(GetAllPermissionModel model, CancellationToken cancellationToken)
        {
            var result = new GetAllPermissionResult();
            var loggedInUserId = _principal.Identity.GetUserId();

            result.RolePermissions = new List<PermissionResult>();

            var roleList = await _context.Roles
                                        .Where(prop => prop.Name != GlobalConstants.CandidateRoleName)
                                        .Where(prop => prop.Name != GlobalConstants.SuperAdminRoleName)
                                        .Where(prop => prop.IsActive)
                                        .ToListAsync();
            var permissionList = await _context.Permission.ToListAsync();
            var rolePermissionList = _context.RolePermission.Where(x => x.RoleId == model.RoleId).ToList();

            result.RoleResult = _mapper.Map<IEnumerable<RoleResult>>(roleList);

            var permissionResult = _mapper.Map<IEnumerable<PermissionResult>>(permissionList);

            foreach (var item in permissionResult)
            {
                item.IsChecked = rolePermissionList.Any(prop => prop.PermissionId == item.Id);

                result.RolePermissions.Add(item);
            }

            return result;
        }
    }
}
