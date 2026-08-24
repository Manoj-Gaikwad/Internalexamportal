using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internalexamportal.Core.Features;
using Internalexamportal.Core.Features.Admin;
using Internalexamportal.Core.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Internalexamportal.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QuestionController(IMediator mediator) => _mediator = mediator;


        [HttpPost("AddSubject")]
        public async Task<IActionResult> AddSubject([FromBody]SubjectModel subjectModel)
        {
            var response = await _mediator.Send(subjectModel);
            return Ok(response);

        }

        [HttpPut("EditSubject")]
        public async Task<IActionResult> EditSubject([FromBody]EditSubjectModel editSubjectModel)
        {
            var response = await _mediator.Send(editSubjectModel);
            return Ok(response);
        }


        [HttpGet("EditQuestion/{Id}")]
        public async Task<IActionResult> EditQuestion(int id)
        {
            var data = new EditQuestionModel { Id = id };
            var response = await _mediator.Send(data);
            return Ok(response);
        }

        [HttpPut("EditQuestion")]
        public async Task<IActionResult> UpdateQuestion([FromBody]UpdateQuestionModel updateQuestionModel)
        {
            var response = await _mediator.Send(updateQuestionModel);
            return Ok(response);
        }

        [HttpGet("GetQuestion")]
        public async Task<IActionResult> GetQuestion()
        {
            var response = await _mediator.Send(new GetQuestionModel());
            return Ok(response);
        }

        [HttpPost("SaveQuestion")]
        public async Task<IActionResult> SaveQuestion([FromBody]AddQuestionRequestModel questionModel)
        {

            var response = await _mediator.Send(questionModel);
            return Ok(response);
        }


        [HttpPost("AddTopic")]
        public async Task<IActionResult> AddTopic([FromBody]SubjectTopicModule TopicForm)
        {
            var response = await _mediator.Send(TopicForm);
            return Ok(response);

        }

        [HttpPost("EditTopic")]
        public async Task<IActionResult> EditTopic([FromBody]EditSubjectTopicModel TopicForm)
        {
            var response = await _mediator.Send(TopicForm);
            return Ok(response);

        }

        [HttpDelete("DeleteQuestion/{Id}")]
        public async Task<IActionResult> DeleteQuestion(string id)
        {
            var data = new DeleteQuestionModel { Id = id };
            var result = await _mediator.Send(data);
            return Ok(result);
        }

        [HttpPost("DeleteQuestionFromTest")]
        public async Task<IActionResult> DeleteQuestionFromTest(DeleteQuestionFromTestModel Id)
        {
            var result = await _mediator.Send(Id);
            return Ok(result);
        }


        [HttpGet("GetSubject")]
        public async Task<IActionResult> GetSubject()
        {
            var response = await _mediator.Send(new GetSubjectModel());
            return Ok(response);
        }

        [HttpGet("GetAllSubjectsWithTopics")]
        public async Task<IActionResult> GetAllSubjectsWithTopics()
        {
            var response = await _mediator.Send(new GetAllSubjectsWithTopicsModel());
            return Ok(response);
        }


        [HttpGet("GetSubjectDetails/{Id}")]
        public async Task<IActionResult> GetSubjectDetails([FromRoute]int id)
        {
            var data = new GetSubjectDetailsModel { Id = id };
            var response = await _mediator.Send(data);
            return Ok(response);
        }

        [HttpGet("GetTopic/{SubjectId}")]
        public async Task<IActionResult> GetTopic([FromRoute]int SubjectId)
        {
            var response = await _mediator.Send(new GetTopicModel { SubjectId = SubjectId });
            return Ok(response);
        }


        [HttpDelete("DeleteSubject/{Id}")]
        public async Task<IActionResult> DeleteSubject([FromRoute] int Id)
        {
            var result = await _mediator.Send(new DeleteSubjectModel { Id = Id });
            return Ok(result); ;
        }


        [HttpGet("GetSubjectTopic")]
        public async Task<IActionResult> GetSubjectTopic()
        {
            var allRoles = await _mediator.Send(new GetSubjectTopicModel());

            return Ok(allRoles);
        }

        [HttpGet("GetQuestion/{SubjectId}/{SubjectTopicId}/{QuestionTypeId}")]
        public async Task<IActionResult> GetQuestion([FromRoute]FilterQuestionModel Form)
        {
            var response = await _mediator.Send(Form);
            return Ok(response);
        }

        [HttpPost("GetQuestions")]
        public async Task<IActionResult> GetQuestions([FromBody] GetQuestionsModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpGet("GetAllDifficult")]
        public async Task<IActionResult> GetAllDifficult()
        {
            var difficultyLevels = await _mediator.Send(new GetDifficultLevelModel());
            return Ok(difficultyLevels);
        }


        [HttpGet("GetQuestionType")]
        public async Task<IActionResult> GetQuestionType()
        {
            var questionTypes = await _mediator.Send(new GetQuestionTypeModel());
            return Ok(questionTypes);
        }

        [HttpGet("GetCorrectOption")]
        public async Task<IActionResult> GetCorrectOption()
        {
            var correctOptions = await _mediator.Send(new GetCorrectOptionModel());
            return Ok(correctOptions);
        }

    }
}