using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Threading;
using Internalexamportal.DataAccessLayer.Entities;

namespace Internalexamportal.Core.Features.DeleteRole
{

    //Model
    public class DeleteRoleModel : IRequest<IdentityResult>
    {
        public string Id { get; set; }
    }

    //Handler
    public class DeleteRoleHandler : IRequestHandler<DeleteRoleModel, IdentityResult>
    {
        private readonly RoleManager<Role> _roleManager;
        public DeleteRoleHandler(
            RoleManager<Role> roleManager
            )
        {
            _roleManager = roleManager;
        }
        public async Task<IdentityResult> Handle(DeleteRoleModel model, CancellationToken token)
        {
            IdentityResult result = new IdentityResult();
            
            //Find role by Id
            var role = await _roleManager.FindByIdAsync(model.Id);

            //If found
            if (role != null)
            {
                //delete the role
                result = await _roleManager.DeleteAsync(role);
            }

            //return result
            return result;
        }
    }
}

