using AutoMapper;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Candidate
{
    public class ExamResultModel : IRequest<CandidateExamResult>
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int QuestionOptionId { get; set; }
        public int AttemptedQuestion { get; set; }
        public int SkippedQuestion { get; set; }
        public int ReviewedQuestion { get; set; }
        public int CorrectQuestion { get; set; }
        public int InCorrectQuestion { get; set; }
        public int MaximumMark { get; set; }
        public int RightMark { get; set; }
        public int NegativeMark { get; set; }
        public int TotalMark { get; set; }
        public string UserId { get; set; }
        public int TestId { get; set; }
        public List<CompletedTestDetails> CompletedTestDetails { get; set; }
    }

    public class CandidateExamResult
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int QuestionOptionId { get; set; }
        public int AttemptedQuestion { get; set; }
        public int SkippedQuestion { get; set; }
        public int ReviewedQuestion { get; set; }
        public int CorrectQuestion { get; set; }
        public int InCorrectQuestion { get; set; }
        public int MaximumMark { get; set; }
        public int RightMark { get; set; }
        public int NegativeMark { get; set; }
        public int TotalMark { get; set; }
        public string UserId { get; set; }
        public int TestId { get; set; }
    }

    public class ExamResultHandler : IRequestHandler<ExamResultModel, CandidateExamResult>
    {

        private readonly IRepository<CompletedTestDetails> _completedTestDetails;
        private readonly IRepository<SubmittedTest> _submittedTest;
        private readonly IRepository<QuestionMark> _questionMark;
        private readonly IRepository<QuestionOption> _questionOption;
        private readonly IMapper _mapper;

        public ExamResultHandler(
            IRepository<CompletedTestDetails> completedTestDetails,
            IRepository<SubmittedTest> submittedTest,
            IRepository<QuestionMark> questionMark,
            IRepository<QuestionOption> questionOption,
            IMapper mapper)
        {
            _completedTestDetails = completedTestDetails;
            _submittedTest = submittedTest;
            _questionMark = questionMark;
            _questionOption = questionOption;
            _mapper = mapper;

        }

        public async Task<CandidateExamResult> Handle(ExamResultModel request, CancellationToken cancellationToken)
        {
            var examresult = new CandidateExamResult();
            var submitTest = new SubmittedTest();
            submitTest.AttemptedQuestion = request.AttemptedQuestion;
            submitTest.SkippedQuestion = request.SkippedQuestion;
            submitTest.ReviewedQuestion= request.ReviewedQuestion;
            submitTest.RightMark= request.RightMark;
            submitTest.CorrectQuestion= request.CorrectQuestion;
            submitTest.InCorrectQuestion= request.InCorrectQuestion;
            submitTest.NegativeMark= request.NegativeMark;
            submitTest.TestId = 1;
            submitTest.MaximumMark= request.MaximumMark;
            submitTest.TotalMark= request.TotalMark;
            submitTest.UserId= request.UserId;
            var submitTests = _mapper.Map<SubmittedTest>(submitTest);
            _submittedTest.Add(submitTest);
            _submittedTest.SaveChanges();

            foreach (var item in request.CompletedTestDetails)
            {
                var setOption = new CompletedTestDetails();
                setOption.QuestionId = item.QuestionId;
                setOption.QuestionOptionId = item.QuestionOptionId;
                setOption.SubmittedTestId = submitTest.Id;
                var completedTest = _mapper.Map<CompletedTestDetails>(setOption);
                _completedTestDetails.Add(setOption);
                _completedTestDetails.SaveChanges();

            }
            return examresult;
        }
    }
}
