using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Features.Admin.Dashboard;
using Internalexamportal.Core.Features.Candidate;
using Internalexamportal.Core.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Internalexamportal.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator) => _mediator = mediator;

        [HttpGet("GetDashboardData")]
        public async Task<IActionResult> GetDashboardData()
        {
            var response = await _mediator.Send(new GetDashboardDataModel());
            return Ok(response);
        }

        [HttpPost("GetAllAdmins")]
        public async Task<IActionResult> GetAllAdmins([FromBody]AdminGridModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpGet("GetUser/{Id}")]
        public async Task<IActionResult> GetUser([FromRoute] string Id)
        {
            var response = await _mediator.Send(new GetUserModel {Id  = Id });
            return Ok(response);

        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);

        }
    }
}