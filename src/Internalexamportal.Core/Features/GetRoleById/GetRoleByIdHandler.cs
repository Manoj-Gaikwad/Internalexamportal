using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Threading;
using Internalexamportal.DataAccessLayer.Entities;

namespace Internalexamportal.Core.Features.GetRoleById
{

    //Model
    public class GetRoleByIdModel : IRequest<GetRoleByIdResult>
    {
        public string Id { get; set; }
    }

    //Result
    public class GetRoleByIdResult
    {
       public string Role { get; set; }
    }

    //Handler
    public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdModel, GetRoleByIdResult>
    {
        private readonly RoleManager<Role> _roleManager;
        public GetRoleByIdHandler(
            RoleManager<Role> roleManager
        )
        {
            _roleManager = roleManager;
        }
        public async Task<GetRoleByIdResult> Handle(GetRoleByIdModel model, CancellationToken token)
        {
            GetRoleByIdResult result = new GetRoleByIdResult();

            //Find existing role
            var role = await _roleManager.FindByIdAsync(model.Id);

            //Return when found.
            result.Role = role.Name;

            return result;
        }
    }
}

