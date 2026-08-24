using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internalexamportal.Core.Features;
using Internalexamportal.Core.Features.Admin;
using Internalexamportal.Core.Features.Admin.testManagement;
using Internalexamportal.Core.Features.AnswerEvaluation;
using Internalexamportal.Core.Features.PrintFeatures;
using Internalexamportal.Core.Features.SubmittedTests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Internalexamportal.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TestController(IMediator mediator) => _mediator = mediator;

        [HttpGet("GetInstruction")]
        public async Task<IActionResult> GetQuestion()
        {
            var response = await _mediator.Send(new GetInstructionModel());
            return Ok(response);
        }

        [HttpGet("GetTestSettings/{Id}")]
        public async Task<IActionResult> GetTestSettings(GetTestSettingsRequestModel Id)
        {
            var response = await _mediator.Send(Id);
            return Ok(response);
        }

        [HttpGet("GetTestSettingType")]
        public async Task<IActionResult> GetTestSettingType()
        {
            var SettingType = await _mediator.Send(new GetTestSettingTypeModel());
            return Ok(SettingType);
        }

        [HttpGet("GetTestTypes")]
        public async Task<IActionResult> GetTestTypes()
        {
            var response = await _mediator.Send(new GetTestTypesModel());
            return Ok(response);
        }

        [HttpPost("GetTests")]
        public async Task<IActionResult> GetTests([FromBody]GetTestModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }
        
        [HttpGet("GetTestCodeType")]
        public async Task<IActionResult> GetTestCodeType()
        {
            var response = await _mediator.Send(new GetTestCodeTypeModel());
            return Ok(response);
        }

        [HttpGet("GetTestDetails/{TestId}")]
        public async Task<IActionResult> GetTestById([FromRoute]GetTestDetailsModel Id)
        {
            var response = await _mediator.Send(Id);
            return Ok(response);
        }

        [HttpPost("UpdateTestDetails")]
        public async Task<IActionResult> UpdateTestDetails([FromBody] UpdateTestDetailsModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpGet("GetTest/{Id}")]
        public async Task<IActionResult> GetTestByTestId(GetTestByTestId Id)
        {
            var response = await _mediator.Send(Id);
            return Ok(response);
        }

        [HttpPost("CheckIfTestNameExists")]
        public async Task<IActionResult> CheckifUserNameExists(CheckIfTestNameExistsRequest model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }


        [HttpPost("GetTestResults")]
        public async Task<IActionResult> GetTestResultsByTestId([FromBody]GetTestResultsRequestModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpPost("EmailResultToCandidate")]
        public async Task<IActionResult> EmailResultToCandidate([FromBody]EmailResultToCandidateRequest model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpPost("SendExamResultEmail")]
        public async Task<IActionResult> SendExamResultEmail([FromBody]SendExamResultEmailRequest TestId)
        {
            var response = await _mediator.Send(TestId);
            return Ok(response);
        }

        [HttpGet("GetTestResultById/{TestResultId}")]
        public async Task<IActionResult> GetTestResultById([FromRoute]int testResultId)
        {
            var request = new GetTestResultByIdRequestModel { TestResultId = testResultId };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("EditTest/{Id}")]
        public async Task<IActionResult> EditTest(EditTestModel Id)
        {
            var response = await _mediator.Send(Id);
            return Ok(response);
        }

        [HttpPost("SaveTest")]
        public async Task<IActionResult> SaveTest([FromBody]TestManagerModel testManagerModel)
        {
            var response = await _mediator.Send(testManagerModel);
            return Ok(response);
        }

        [HttpPut("EditTest")]
        public async Task<IActionResult> UpdateTest([FromBody]UpdateTestModel updateTestModel)
        {
            var response = await _mediator.Send(updateTestModel);
            return Ok(response);
        }

        [HttpDelete("DeleteTest/{Id}")]
        public async Task<IActionResult> DeleteTest([FromRoute] string Id)
        {
            var response = await _mediator.Send(new DeleteTestModel{ Id = Id });
            return Ok(response);
        }

        [HttpDelete("DeleteInstruction/{Id}")]
        public async Task<IActionResult> DeleteInstruction([FromRoute] string Id)
        {
            var response = await _mediator.Send(new DeleteInstructionModel { Id = Id });
            return Ok(response);
        }

        [HttpGet("GetTestSettingTypes/{TestId}")]
        public async Task<IActionResult> GetTestSettingTypes([FromRoute] GetTestSettingTypesModel testSettingModel)
        {
            var response = await _mediator.Send(testSettingModel);
            return Ok(response);
        }

        [HttpPost("TestSetting")]
        public async Task<IActionResult> ActivationCode([FromBody]TestSettingModel testSettingModel)
        {
            var response = await _mediator.Send(testSettingModel);
            return Ok(response);
        }

        [HttpPut("UpdateTestSetting")]
        public async Task<IActionResult> UpdateTestSetting([FromBody]UpdateTestSettingModel updateTestSettingModel)
        {
            var response = await _mediator.Send(updateTestSettingModel);
            return Ok(response);
        }

        [HttpPost("AddTestQuestion")]
        public async Task<IActionResult> AddTestQuestion([FromBody]AddTestQuestionModel addTestQuestionModel)
        {
            var response = await _mediator.Send(addTestQuestionModel);
            return Ok(response);
        }

        [HttpPost("UpdateTestQuestion")]
        public async Task<IActionResult> UpdateTestQuestion([FromBody]UpdateTestQuestionModel updateTestQuestionModel)
        {
            var response = await _mediator.Send(updateTestQuestionModel);
            return Ok(response);
        }

        [HttpGet("GetTestQuestion/{Id}")]
        public async Task<IActionResult> GetTestQuestion([FromRoute] GetTestQuestionModel testId)
        {
            var response = await _mediator.Send(testId);
            return Ok(response);
        }

        [HttpPost("PublishTest")]
        public async Task<IActionResult> ActivationCode([FromBody]PublishTestModel publishTestModel)
        {
            var response = await _mediator.Send(publishTestModel);
            return Ok(response);
        }

        [HttpPut("UpdateTestPublish")]
        public async Task<IActionResult> UpdateTestPublish([FromBody]UpdateTestPublicModel updateTestpublishModel)
        {
            var response = await _mediator.Send(updateTestpublishModel);
            return Ok(response);
        }

        [HttpPost("ActivationCode")]
        public async Task<IActionResult> ActivationCode([FromBody]ActivationCodeModel activationCode)
        {
            var response = await _mediator.Send(activationCode);
            return Ok(response);
        }

        [HttpGet("AllowTestResume/{SubmittedTestId}")]
        public async Task<IActionResult> GetSubmittedAnswers([FromRoute] int SubmittedTestId)
        {
            var request = new AllowTestResumeRequestModel { SubmittedTestId = SubmittedTestId };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("UpdateActivationCode")]
        public async Task<IActionResult> UpdateActivationCode([FromBody]UpdateActivationCodeModel activationCode)
        {
            var response = await _mediator.Send(activationCode);
            return Ok(response);
        }

        [HttpPost("SaveInstruction")]
        public async Task<IActionResult> SaveInstruction([FromBody]TestInstructionModel testManagerModel)
        {
            var response = await _mediator.Send(testManagerModel);
            return Ok(response);
        }

      

        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail([FromBody]EmailExamLinkModel emailExamLink)
        {
            var response = await _mediator.Send(emailExamLink);
            return Ok(response);
        }

        [HttpGet("GetFilteredQuestionsForTest/{TestId}/{SubjectId}/{SubjectTopicId}/{QuestionTypeId}")]
        public async Task<IActionResult> GetFilteredQuestionsForTest([FromRoute] GetFilteredQuestionForTestModel request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("PrintCandidateResult/{Id}")]
        public async Task<IActionResult> PrintMarksheet([FromRoute] int  id)
        {
            var data = new PrintCandidateResultModel { Id = id };
            var result = await _mediator.Send(data);
            return File(result.DocumentBlob, result.MimeType, result.FileName);
        }

        [HttpGet("PrintCertificate/{Id}")]
        public async Task<IActionResult> PrintCertificate([FromRoute] int id)
        {
            var data = new PrintCertificateModel { Id = id };
            var result = await _mediator.Send(data);
            return File(result.DocumentBlob, result.MimeType, result.FileName);
        }

        [HttpGet("GetSubmittedAnswers/{SubmittedTestId}")]
        public async Task<IActionResult> GetSubmittedAnswers([FromRoute]GetSubmittedAnswersRequestModel SubmittedTestId)
        {
            var response = await _mediator.Send(SubmittedTestId);
            return Ok(response);
        }

        [HttpPost("SaveSubjectiveMarks")]
        public async Task<IActionResult> SaveSubjectiveMarks([FromBody] SaveSubjectiveMarksRequestModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpPost("SaveQuestionEvaluation")]
        public async Task<IActionResult> SaveSubjectiveMarks([FromBody] SaveQuestionEvaluationRequestModel model)
        {
            var response = await _mediator.Send(model);
            return Ok(response);
        }

        [HttpGet("PrintTestReport/{Id}")]
        public async Task<IActionResult> PrintTestReport([FromRoute] PrintTestReportModel model)
        {
            var result = await _mediator.Send(model);
            return File(result.DocumentBlob, result.MimeType, result.FileName);
        }

        [HttpGet("PrintTestReportExcel/{id}")]
        public async Task<IActionResult> PrintTestReportExcel(int id)
        {
            var result = await _mediator.Send(new PrintTestReportExcelModel { Id = id });
            return File(result.DocumentBlob, result.MimeType, result.FileName);
        }

    }
}