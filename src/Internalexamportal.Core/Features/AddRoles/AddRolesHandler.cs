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
    public class AddRolesModel : IRequest<AddRolesResult>
    {
        public List<string> Roles { get; set; }
    }

    //Result
    public class AddRolesResult
    {
      public bool Succeeded { get; set; }
    }

    //Handler
    public class AddRolesHandler : IRequestHandler<AddRolesModel, AddRolesResult>
    {
        private readonly RoleManager<Role> _roleManager;
        public AddRolesHandler(
            RoleManager<Role> roleManager
        )
        {
            _roleManager = roleManager;
        }
        public async Task<AddRolesResult> Handle(AddRolesModel model, CancellationToken token)
        {
            AddRolesResult result = new AddRolesResult();

            //Iterate all string in array
            foreach (var role in model.Roles)
            {
                //Check if the rolename alreay exists.
                var roleExists = await _roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    var addRole = new Role { Name = role ,IsActive = true, CreatedDate = Utilities.GetISTDateTime()};

                    //If the role doesnot exist, add in the database. 
                    await _roleManager.CreateAsync(addRole);

                    //set succeeded flag to true. 
                    result.Succeeded = true;
                }
            }

            //return result
            return result;
        }
    }
}
