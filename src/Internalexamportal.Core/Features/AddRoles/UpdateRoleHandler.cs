using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Internalexamportal.DataAccessLayer.Entities;
using Internalexamportal.Core.Commons.Utils;

namespace Internalexamportal.Core.Features.AddRoles
{

    //Model
    public class UpdateRoleModel : IRequest<UpdateRoleResult>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    //Result
    public class UpdateRoleResult
    {
        public bool Succeeded { get; set; }
    }

    //Handler
    public class UpdateRoleHandler : IRequestHandler<UpdateRoleModel, UpdateRoleResult>
    {
        private readonly RoleManager<Role> _roleManager;
        public UpdateRoleHandler(
            RoleManager<Role> roleManager
        )
        {
            _roleManager = roleManager;
        }
        public async Task<UpdateRoleResult> Handle(UpdateRoleModel model, CancellationToken token)
        {
            UpdateRoleResult result = new UpdateRoleResult();

            //Check if the role alreay exists.
            var roleExists = await _roleManager.RoleExistsAsync(model.Name);

            if (!roleExists)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);

                role.Name = model.Name;
                //If the role doesnot exist, add in the database. 
                await _roleManager.UpdateAsync(role);

                //set succeeded flag to true. 
                result.Succeeded = true;
            }


            //return result
            return result;
        }
    }
}
