using AutoMapper;
using Internalexamportal.Common.Extensions;
using Internalexamportal.Core.Enums;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Candidate
{
    public class GetQuestionPaperModel : IRequest<GetQuestionPaperResult>
    {
        public int Id { get; set; }
        public bool IsPendingTest { get; set; }
        public int SubmittedTestId { get; set; }
    }

    public class GetQuestionPaperResult
    {
        public ICollection<QuestionsList> QuestionsList { get; set; }
        public List<SubjectiveAnswerModel> SubjectiveAnswers { get; set; }
    }

    public class QuestionsList
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public QuestionModel Question { get; set; }
        public int PositiveMark { get; set; }
        public int NegativeMark { get; set; }
        public bool IsMultiSelect { get; set; }
        public double TimeTaken { get; set; }
        public bool Answered { get; set; }
    }

    public class QuestionModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int QuestionTypeId { get; set; }
        public IEnumerable<QuestionOptionModel> QuestionOption { get; set; }
    }

    public class QuestionOptionModel
    {
        public int Id { get; set; }
        public String OptionText { get; set; }
        public int QuestionId { get; set; }
        public bool IsChecked { get; set; }
    }

    public class GetQuestionPaperHandler : IRequestHandler<GetQuestionPaperModel, GetQuestionPaperResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public GetQuestionPaperHandler(IMapper mapper, InternalExamportalContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<GetQuestionPaperResult> Handle(GetQuestionPaperModel request, CancellationToken cancellationToken)
        {
            var result = new GetQuestionPaperResult();
            result.SubjectiveAnswers = new List<SubjectiveAnswerModel>();

            var questionOptionData = await _dbContext.TestQuestion
                                                .Where(prop => prop.TestId == request.Id)
                                                .Include(prop => prop.Question)
                                                .ThenInclude(prop => prop.QuestionOption)
                                                .AsNoTracking()
                                                .ToListAsync();

            var testSetting = _dbContext.Setting.Where(pro => pro.TestId == request.Id && pro.TestSettingTypeId == (int)TestSettingEnum.Shuffling).FirstOrDefault();
            List<QuestionsList> data;
            if (testSetting?.IsSettingApplied == true) // Safe navigation to avoid null ref
            {
                data = _mapper.Map<List<QuestionsList>>(questionOptionData.Randomize());
            }
            else
            {
                data = _mapper.Map<List<QuestionsList>>(questionOptionData);
            }


            List<SubmittedOption> submittedOptions = new List<SubmittedOption>();
            List<SubjectiveAnswer> subjectiveAnswers = new List<SubjectiveAnswer>();
            if (request.IsPendingTest)
            {
                submittedOptions = await _dbContext.SubmittedOption
                                        .Where(prop => prop.SubmittedTestId == request.SubmittedTestId)
                                        .ToListAsync();

                subjectiveAnswers = await _dbContext.SubjectiveAnswer
                                        .Where(prop => prop.SubmittedTestId == request.SubmittedTestId)
                                        .ToListAsync();

            }
            
            int i = 1;
            foreach (var testQuestion in data)
            {
                testQuestion.Id = i;
                i++;

                if (request.IsPendingTest)
                {
                    if (testQuestion.Question.QuestionTypeId == (int)QuestionTypeEnum.Subjective)
                    {
                        var answer = subjectiveAnswers.Where(prop => prop.QuestionId == testQuestion.QuestionId).FirstOrDefault();
                        if (answer != null)
                        {
                            result.SubjectiveAnswers.Add(new SubjectiveAnswerModel
                            {
                                QuestionId = testQuestion.QuestionId,
                                AnswerText = answer.Answer
                            });
                            testQuestion.Answered = true;
                            testQuestion.TimeTaken = answer.TimeTaken;
                        }
                    }
                    else
                    {
                        var answers = submittedOptions.Where(prop => prop.QuestionId == testQuestion.QuestionId).ToList();
                        if (answers.Count() > 0)
                        {
                            foreach(var option in testQuestion.Question.QuestionOption)
                            {
                                if(answers.Any(prop => prop.QuestionOptionId == option.Id))
                                {
                                    option.IsChecked = true;
                                }
                            }
                            testQuestion.Answered = true;
                            testQuestion.TimeTaken = answers[0].TimeTaken;
                        }
                    }
                }
            }

            result.QuestionsList = data;
            return result;

        }
    }
}
