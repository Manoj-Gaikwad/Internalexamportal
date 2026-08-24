using System.Threading.Tasks;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Features.AddRoles;
using Internalexamportal.Core.Features.DeleteRole;
using Internalexamportal.Core.Features.GetRoleById;
using Internalexamportal.Core.Features.Privilege;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Internalexamportal.Web.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator) => _mediator = mediator;

        // GET: api/role
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var allRoles = await _mediator.Send(new GetAllRolesModel());

            return Ok(allRoles);
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> SaveRolePermission([FromBody] AddRoleModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpPost("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpGet("UpdateRoleStatus/{RoleId}")]
        public async Task<IActionResult> UpdateRoleStatus([FromRoute] UpdateRoleStatusModel Id)
        {
            var response = await _mediator.Send(Id);
            return Ok(response);

        }

        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var response = await _mediator.Send(new GetRolesModel());
            return Ok(response);
        }


        [HttpGet("GetAllPermissions/{RoleId}")]
        public async Task<IActionResult> GetAllPermissions(GetAllPermissionModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }


        [HttpPost("SaveRolePermission")]
        public async Task<IActionResult> SaveRolePermission([FromBody]SaveRolePermissionModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        // GET api/role/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById([FromRoute] GetRoleByIdModel getRoleByIdModel)
        {
            var roleById = await _mediator.Send(getRoleByIdModel);

            return Ok(roleById);
        }

        // POST api/roles
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddRolesModel addRolesModel)
        {
            var result = await _mediator.Send(addRolesModel);

            if (!result.Succeeded) return BadRequest(result);

            return CreatedAtAction("Post", result);
            
        }

        // DELETE api/role/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteRoleModel deleteRoleModel)
        {
            var result = await _mediator.Send(deleteRoleModel);

            if (!result.Succeeded) return NotFound(result.Errors);

            return NoContent();
        }
    }
}
