using AutoMapper;
using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Privilege
{
    //Model
    public class GetAllRolesModel : IRequest<GetAllRolesResult>
    {

    }

    //Result
    public class GetAllRolesResult
    {
        public List<RoleResult> Roles { get; set; }
    }

    //Handler
    public class GetAllRolesHandler : IRequestHandler<GetAllRolesModel, GetAllRolesResult>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public GetAllRolesHandler(RoleManager<Role> roleManager, UserManager<User> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<GetAllRolesResult> Handle(GetAllRolesModel model, CancellationToken cancellationToken)
        {
            var result = new GetAllRolesResult();

            var roles = _roleManager.Roles;

            result.Roles = _mapper.Map<List<RoleResult>>(roles);

            foreach (var role in result.Roles)
            {
                role.Count = (await _userManager.GetUsersInRoleAsync(role.Name)).Count;
            }

            return result;
        }
    }
}
