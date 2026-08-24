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
    public class AddRoleModel : IRequest<AddRoleResult>
    {
        public string Name { get; set; }
    }

    //Result
    public class AddRoleResult
    {
        public bool Succeeded { get; set; }
    }

    //Handler
    public class AddRoleHandler : IRequestHandler<AddRoleModel, AddRoleResult>
    {
        private readonly RoleManager<Role> _roleManager;
        public AddRoleHandler(
            RoleManager<Role> roleManager
        )
        {
            _roleManager = roleManager;
        }
        public async Task<AddRoleResult> Handle(AddRoleModel model, CancellationToken token)
        {
            AddRoleResult result = new AddRoleResult();

            //Check if the rolename alreay exists.
            var roleExists = await _roleManager.RoleExistsAsync(model.Name);

            if (!roleExists)
            {
                var addRole = new Role { Name = model.Name, IsActive = true, CreatedDate = Utilities.GetISTDateTime() };

                //If the role doesnot exist, add in the database. 
                await _roleManager.CreateAsync(addRole);

                //set succeeded flag to true. 
                result.Succeeded = true;
            }


            //return result
            return result;
        }
    }
}
