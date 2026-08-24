using AutoMapper;
using Internalexamportal.Core.Enums;
using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.AnswerEvaluation
{
    public class GetSubmittedAnswersRequestModel : IRequest<GetSubmittedAnswersResult>
    {
        public int SubmittedTestId { get; set; }
    }

    public class GetSubmittedAnswersResult
    {
        public int Id { get; set; }
        public DateTime SubmitDate { get; set; }
        public int ObtainedMark { get; set; }
        public int RightMark { get; set; }
        public int NegativeMark { get; set; }
        public int TotalMark { get; set; }
        public string UserName { get; set; }
        public string TestName { get; set; }
        public List<ObjectiveQuestionModel> Objective { get; set; }
        public List<SubjectiveQuestionModel> Subjective { get; set; }

    }

    public class SubjectiveQuestionModel
    {
        public int Id { get; set; }
        public int QuestionType { get; set; }
        public string Question { get; set; }
        public QuestionOptionModel QuestionOption { get; set; }
        public string SubmittedAnswer { get; set; }
        public int PositiveMarks { get; set; }
        public int NegativeMarks { get; set; }
        public int? ObtainedMarks { get; set; }
        public string Comment { get; set; }
    }

    public class ObjectiveQuestionModel
    {
        public int Id { get; set; }
        public int QuestionType { get; set; }
        public string Question { get; set; }
        public List<QuestionOptionModel> QuestionOptions { get; set; }
        public List<int> SelectedOptions { get; set; }
        public int PositiveMarks { get; set; }
        public int NegativeMarks { get; set; }
    }
    public class QuestionOptionModel
    {
        public int Id { get; set; }
        public string OptionText { get; set; }
        public int QuestionId { get; set; }
        public bool IsAnswer { get; set; }
    }

    public class GetSubmittedAnswersHandler : IRequestHandler<GetSubmittedAnswersRequestModel, GetSubmittedAnswersResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetSubmittedAnswersHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetSubmittedAnswersResult> Handle(GetSubmittedAnswersRequestModel request, CancellationToken cancellationToken)
        {

            var submittedTest = await _dbContext.SubmittedTest
                                    .Where(prop => prop.Id == request.SubmittedTestId)
                                    .Include(prop => prop.Test)
                                    .Include(prop => prop.User)
                                    .FirstOrDefaultAsync();

            var result = _mapper.Map<GetSubmittedAnswersResult>(submittedTest);

            var questionList = await _dbContext.TestQuestion
                                        .Where(prop => prop.TestId == submittedTest.TestId)
                                        .Include(prop => prop.Question)
                                            .ThenInclude(prop => prop.QuestionOption)
                                        .ToListAsync();

            result.Subjective = _mapper.Map<List<SubjectiveQuestionModel>>(questionList.Where(q => q.Question.QuestionTypeId == (int)QuestionTypeEnum.Subjective));
            result.Objective = _mapper.Map<List<ObjectiveQuestionModel>>(questionList.Where(q => q.Question.QuestionTypeId != (int)QuestionTypeEnum.Subjective));

            var submittedOptions = await _dbContext.SubmittedOption
                                        .Where(prop => prop.SubmittedTestId == request.SubmittedTestId)
                                        .ToListAsync();

            var subjectiveAnswers = await _dbContext.SubjectiveAnswer
                                        .Where(prop => prop.SubmittedTestId == request.SubmittedTestId)
                                        .ToListAsync();

            foreach (var question in result.Subjective)
            {
                var answer = subjectiveAnswers.Where(prop => prop.QuestionId == question.Id).FirstOrDefault();
                if (answer != null)
                {
                    question.SubmittedAnswer = answer.Answer;
                    question.ObtainedMarks = answer.ObtainedMarks;
                    question.Comment = answer.Comment;
                }
            }

            foreach (var question in result.Objective)
            {
                var answer = submittedOptions.Where(prop => prop.QuestionId == question.Id);
                if (answer != null)
                {
                    question.SelectedOptions = answer.Select(prop => prop.QuestionOptionId).ToList();
                }
            }

            return result;
        }


    }
}
