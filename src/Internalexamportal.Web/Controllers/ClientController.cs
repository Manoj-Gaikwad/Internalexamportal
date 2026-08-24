using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internalexamportal.Core.Features.ClientFeatures;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Internalexamportal.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator) => _mediator = mediator;

        [HttpPost("GetClientsGrid")]
        public async Task<IActionResult> GetAllClients([FromBody]GetAllClientModel model)
        {
            var response = await _mediator.Send(model);

            return Ok(response);
        }

        [HttpGet("Get/{Id}")]
        public async Task<IActionResult> EditCandidate([FromRoute] int Id)
        {
            var response = await _mediator.Send(new GetClientByIdModel { Id = Id });
            return Ok(response);

        }

        [HttpPost("AddEdit")]
        public async Task<IActionResult> AddEdit([FromBody]AddEditClientModel model)
        {
            var response = await _mediator.Send(model);

            return Ok(response);
        }

        [HttpDelete("DeleteClient/{Id}")]
        public async Task<IActionResult> DeleteClient([FromRoute] string Id)
        {
            var result = await _mediator.Send(new DeleteClient { Id = Id });
            return Ok(result); 
        }

        [HttpGet("GetClients")]
        public async Task<IActionResult> GetClients()
        {
            var response = await _mediator.Send(new GetClientsModel());

            return Ok(response);
        }
    }
}
