using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Privilege
{
    //Model
    public class GetRolesModel : IRequest<GetRolesResult>
    {

    }

    //Result
    public class GetRolesResult
    {
        public List<RoleResult> Roles { get; set; }
    }

    public class RoleResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Count { get; set; }
    }

    //Handler
    public class GetRolesHandler : IRequestHandler<GetRolesModel, GetRolesResult>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public GetRolesHandler(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public Task<GetRolesResult> Handle(GetRolesModel model, CancellationToken cancellationToken)
        {
            GetRolesResult result = new GetRolesResult();

            var roles = _roleManager.Roles
                .Where(prop => prop.Name != GlobalConstants.CandidateRoleName)
                .Where(prop => prop.Name != GlobalConstants.SuperAdminRoleName)
                .Where(prop => prop.IsActive);

            result.Roles = _mapper.Map<List<RoleResult>>(roles);

            return Task.FromResult(result);
        }
    }
}
