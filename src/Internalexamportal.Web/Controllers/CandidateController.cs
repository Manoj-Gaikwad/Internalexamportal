using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Internalexamportal.Core.Features.Candidate;
using Internalexamportal.Core.Features.Candidate.Groups;
using Internalexamportal.Core.Features.SubmittedTests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internalexamportal.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CandidateController(IMediator mediator) => _mediator = mediator;

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetCandidate(CandidateGridModel model)
        {
            var response = await _mediator.Send(model);
          
            return Ok(response);
        }

        [HttpPost("AddCandidate")]
        public async Task<IActionResult> AddCandidate(AddCandidateModel model)
        {
            // Use Mediatr to request user creation in business logic
            var response = await _mediator.Send(model);

            // return Ok if success
            if (response.Succeeded) return Ok(response);

            // Add errors to model state with empty key
            // Empty string means generic error in model
            response.Errors
                .ToList()
                .ForEach(x => ModelState.AddModelError(string.Empty, x.Description));

            // return bad request
            return BadRequest(ModelState);
        }

        [HttpGet("EditCandidate/{Id}")]
        public async Task<IActionResult> EditCandidate([FromRoute] string Id)
        {
            var response = await _mediator.Send(new EditCandidateModel { Id = Id });
            return Ok(response);

        }

        [HttpPost("UpdateCandidateData")]
        public async Task<IActionResult> UpdateCandidateData(CandidateDetailModel candidateDetailModel)
        {
            var response = await _mediator.Send(candidateDetailModel);
            return Ok(response);

        }

        [HttpGet("GetCandidateTestsById/{CandidateId}")]
        public async Task<IActionResult> GetCandidateTestsById([FromRoute] GetCandidateTestsByIdRequestModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpGet("GetCandidateTestsCount")]
        public async Task<IActionResult> GetCandidateTestsCount()
        {
            var response = await _mediator.Send(new GetCandidateTestsCountRequestModel());
            return Ok(response);
        }   

        [HttpGet("GetCandidateTests")]
        public async Task<IActionResult> GetCandidateTests()
        {
            var response = await _mediator.Send(new GetCandidateTestsRequestModel());
            return Ok(response);
        }

        [HttpGet("GetUpcomingTests")]
        public async Task<IActionResult> GetUpcomingTests()
        {
            var response = await _mediator.Send(new GetUpcomingTestsRequestModel());
            return Ok(response);
        }

        [HttpDelete("DeleteCandidate/{Id}")]
        public async Task<IActionResult> DeleteCandidate([FromRoute] string Id)
        {
            var result = await _mediator.Send(new CandidateDeleteModel { Id = Id });
            return Ok(result); ;
        }

        [HttpGet("GetTestByActivation/{ActivationTestCode}")] 
        public async Task<IActionResult> GetTestByActivation([FromRoute]GetTestByActivationModel getQuestionPaperModel)
        {
            var response = await _mediator.Send(getQuestionPaperModel);
            return Ok(response);
        }

        [HttpPost("GetQuestionPaper")]
        public async Task<IActionResult> GetQuestionPaper([FromBody]GetQuestionPaperModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpPost("SaveSelectedOptions")]
        public async Task<IActionResult> ExamResult(ExamResultModel examResultModel)
        {
            var response = await _mediator.Send(examResultModel);
            return Ok(response);
        }

        [HttpPost("StartTest")]
        public async Task<IActionResult> StartTest([FromBody]StartTestRequestModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpPost("SaveSubmittedQuestion")]
        public async Task<IActionResult> SaveSubmittedQuestion(SaveSubmittedQuestionRequestModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpPost("SubmitTest")]
        public async Task<IActionResult> SubmitTest(SubmitTestRequestModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpGet("GetInstruction/{Id}")]
        public async Task<IActionResult> GetInstruction(int id)
        {
            var data = new GetTestInstructionModel { Id = id };
            var response = await _mediator.Send(data);
            return Ok(response);
        }

        [HttpGet("GetExamReport/{Id}")]
        public async Task<IActionResult> GetExamReport( int id)
        {
            var data = new GetExamReportRequestModel { Id = id };
            var response = await _mediator.Send(data);
            return Ok(response);
        }

        [HttpPost("AddGroup")]
        public async Task<IActionResult> AddGroup(AddGroupModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpGet("GetAllGroups")]
        public async Task<IActionResult> GetAllGroups()
        {
            var response = await _mediator.Send(new GetAllGroupsModel());
            return Ok(response);
        }

        [HttpDelete("DeleteGroup/{Id}")]
        public async Task<IActionResult> DeleteGroup([FromRoute] int Id)
        {
            var data= new DeleteGroupModel { Id = Id };
            var result = await _mediator.Send(data);
            return Ok(result); ;
        }

        [HttpPost("GetExportCandidateData")]
        public async Task<IActionResult> GetExportCandidateData(ExportCandidatesModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }
    }
}