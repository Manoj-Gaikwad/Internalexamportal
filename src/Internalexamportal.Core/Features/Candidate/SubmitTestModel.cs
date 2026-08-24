using AutoMapper;
using Internalexamportal.DataAccessLayer;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Principal;
using Internalexamportal.Common.Extensions;
using InternalExamportal.DataAccessLayer.Entities;
using Internalexamportal.Core.Commons.Utils;
using Internalexamportal.Core.Enums;

namespace Internalexamportal.Core.Features.Candidate
{
    public class SubmitTestRequestModel : IRequest<SubmitTestResponseModel>
    {
        public string UserId { get; set; }
        public int TestId { get; set; }
        public int AttemptedQuestion { get; set; }
        public int SkippedQuestion { get; set; }
        public int ReviewedQuestion { get; set; }
        public double TimeTaken { get; set; }
        public int SubmittedTestId { get; set; }
        public ICollection<UserOption> UserOptions { get; set; }
        public ICollection<SubjectiveAnswerModel> SubjectiveAnswers { get; set; }

    }

    public class UserOption
    {
        public int QuestionId { get; set; }
        public List<int> Answers { get; set; }
        public double TimeTaken { get; set; }
    }

    public class SubjectiveAnswerModel
    {
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public double TimeTaken { get; set; }
    }

    public class SubmitTestResponseModel
    {
        public bool Success { get; set; }
        public int SubmittedTestId { get; set; }
    }

    public class SubmitTestHandler : IRequestHandler<SubmitTestRequestModel, SubmitTestResponseModel>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPrincipal _principle;
        private int TotalMarks;
        private int RightMarks;
        private int NegativeMarks;
        private int CorrectQuestions;
        private int InCorrectQuestions;
        private int MaximumMarks;

        public SubmitTestHandler(InternalExamportalContext dbContext, IMapper mapper, IPrincipal principal)
        {
            _principle = principal;
            _dbContext = dbContext;
            _mapper = mapper;
            TotalMarks = 0;
            RightMarks = 0;
            NegativeMarks = 0;
            CorrectQuestions = 0;
            InCorrectQuestions = 0;

        }

        public async Task<SubmitTestResponseModel> Handle(SubmitTestRequestModel request, CancellationToken cancellationToken)
        {
            request.UserId = _principle.Identity.GetUserId();

            var test = _dbContext.Test
                .Where(prop => prop.Id == request.TestId)
                .FirstOrDefault();

            MaximumMarks = test.TotalMark;

            var questionList = await _dbContext.TestQuestion
                .Where(prop => prop.TestId == request.TestId)
                .Include(prop => prop.Question)
                    .ThenInclude(prop => prop.QuestionOption)
                .ToListAsync();

            var isSubjective = questionList.Any(prop => prop.Question.QuestionTypeId == (int)QuestionTypeEnum.Subjective);

            foreach (UserOption userOption in request.UserOptions)
            {
                var question = questionList.Where(prop => prop.QuestionId == userOption.QuestionId).FirstOrDefault();
                var answers = question.Question.QuestionOption.Where(prop => prop.IsAnswer)
                                                .Select(prop => prop.Id).OrderBy(prop => prop).ToList();
                var userAnswers = userOption.Answers.OrderBy(prop => prop).ToList();

                if (userAnswers.SequenceEqual(answers))
                {
                    CorrectQuestions++;
                    RightMarks += question.PositiveMark;
                }
                else
                {
                    InCorrectQuestions++;
                    NegativeMarks += question.NegativeMark;
                }
            }

            TotalMarks = RightMarks - NegativeMarks;

            SubmittedTest submittedTest = new SubmittedTest
            {
                Id = request.SubmittedTestId,
                AttemptedQuestion = request.AttemptedQuestion,
                SkippedQuestion = request.SkippedQuestion,
                ReviewedQuestion = request.ReviewedQuestion,
                CorrectQuestion = CorrectQuestions,
                InCorrectQuestion = InCorrectQuestions,
                MaximumMark = MaximumMarks,
                RightMark = RightMarks,
                NegativeMark = NegativeMarks,
                TotalMark = TotalMarks,
                TimeTaken = request.TimeTaken,
                UserId = request.UserId,
                TestId = request.TestId,
                SubmitDate = Utilities.GetISTDateTime(),
                StatusId = isSubjective ? (int)TestStatusEnum.Submitted : (int)TestStatusEnum.Evaluated
            };

            _dbContext.SubmittedTest.Update(submittedTest);
            await _dbContext.SaveChangesAsync();

            List<SubmittedOption> submittedOptions = await _dbContext.SubmittedOption
                                                    .Where(prop => prop.SubmittedTestId == submittedTest.Id)
                                                    .ToListAsync();

            foreach (var options in request.UserOptions)
            {
                foreach (var option in options.Answers)
                {
                    var answer = submittedOptions.Where(prop => prop.QuestionOptionId == option).FirstOrDefault();

                    if (answer != null)
                    {
                        answer.TimeTaken = options.TimeTaken;
                        _dbContext.SubmittedOption.Update(answer);
                    }
                    else
                    {
                        _dbContext.SubmittedOption.Add(new SubmittedOption
                        {
                            SubmittedTestId = submittedTest.Id,
                            QuestionId = options.QuestionId,
                            QuestionOptionId = option,
                            TimeTaken = options.TimeTaken
                        });
                    }
                }

                var optionsToRemove = submittedOptions.Where(prop => prop.QuestionId == options.QuestionId
                                                        && !options.Answers.Contains(prop.QuestionOptionId));
                _dbContext.RemoveRange(optionsToRemove);
            }

            List<SubjectiveAnswer> subjectiveAnswers = await _dbContext.SubjectiveAnswer
                                                    .Where(prop => prop.SubmittedTestId == submittedTest.Id)
                                                    .ToListAsync();

            foreach (var subjectiveAnswer in request.SubjectiveAnswers)
            {
                var answer = subjectiveAnswers.Where(prop => prop.QuestionId == subjectiveAnswer.QuestionId).FirstOrDefault();

                if (answer != null)
                {
                    answer.TimeTaken = subjectiveAnswer.TimeTaken;
                    answer.Answer = subjectiveAnswer.AnswerText;
                    _dbContext.SubjectiveAnswer.Update(answer);
                }
                else
                {
                    _dbContext.SubjectiveAnswer.Add(new SubjectiveAnswer
                    {
                        Answer = subjectiveAnswer.AnswerText,
                        QuestionId = answer.QuestionId,
                        SubmittedTestId = submittedTest.Id,
                        TimeTaken = subjectiveAnswer.TimeTaken
                    });
                }
            }

            await _dbContext.SaveChangesAsync();

            return new SubmitTestResponseModel { Success = true, SubmittedTestId = submittedTest.Id };
        }
    }
}
