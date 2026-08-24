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

namespace Internalexamportal.Core.Features.SubmittedTests
{
    public class SaveSubmittedQuestionRequestModel : IRequest<SaveSubmittedQuestionResponseModel>
    {
        public double TimeTaken { get; set; }
        public int SubmittedTestId { get; set; }
        public int QuestionType { get; set; }
        public ObjectiveAnswerModel ObjectiveAnswer { get; set; }
        public SubjectiveAnswerModel SubjectiveAnswer { get; set; }

    }

    public class ObjectiveAnswerModel
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

    public class SaveSubmittedQuestionResponseModel
    {
        public bool Success { get; set; }
    }

    public class SaveSubmittedQuestionHandler : IRequestHandler<SaveSubmittedQuestionRequestModel, SaveSubmittedQuestionResponseModel>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPrincipal _principle;

        public SaveSubmittedQuestionHandler(InternalExamportalContext dbContext, IMapper mapper, IPrincipal principal)
        {
            _principle = principal;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<SaveSubmittedQuestionResponseModel> Handle(SaveSubmittedQuestionRequestModel request, CancellationToken cancellationToken)
        {
            if (request.QuestionType == (int)QuestionTypeEnum.Subjective)
            {
                SubjectiveAnswer subjectiveAnswer = await _dbContext.SubjectiveAnswer
                                                    .Where(prop => prop.SubmittedTestId == request.SubmittedTestId
                                                    && prop.QuestionId == request.SubjectiveAnswer.QuestionId)
                                                    .FirstOrDefaultAsync();
                if (subjectiveAnswer == null)
                {
                    subjectiveAnswer = new SubjectiveAnswer
                    {
                        Answer = request.SubjectiveAnswer.AnswerText,
                        QuestionId = request.SubjectiveAnswer.QuestionId,
                        SubmittedTestId = request.SubmittedTestId,
                        TimeTaken = request.SubjectiveAnswer.TimeTaken
                    };
                    await _dbContext.SubjectiveAnswer.AddAsync(subjectiveAnswer);
                }
                else
                {
                    subjectiveAnswer.Answer = request.SubjectiveAnswer.AnswerText;
                    subjectiveAnswer.TimeTaken = request.SubjectiveAnswer.TimeTaken;
                    _dbContext.SubjectiveAnswer.Update(subjectiveAnswer);
                }
            }
            else
            {
                List<SubmittedOption> objectiveAnswers = await _dbContext.SubmittedOption
                                                    .Where(prop => prop.SubmittedTestId == request.SubmittedTestId
                                                    && prop.QuestionId == request.ObjectiveAnswer.QuestionId)
                                                    .ToListAsync();
                if (objectiveAnswers.Count > 0)
                {
                    _dbContext.SubmittedOption.RemoveRange(objectiveAnswers);
                    await _dbContext.SaveChangesAsync();
                }

                List<SubmittedOption> submittedOptions = new List<SubmittedOption>();

                foreach (var option in request.ObjectiveAnswer.Answers)
                {
                    submittedOptions.Add(new SubmittedOption
                    {
                        SubmittedTestId = request.SubmittedTestId,
                        QuestionId = request.ObjectiveAnswer.QuestionId,
                        QuestionOptionId = option,
                        TimeTaken = request.ObjectiveAnswer.TimeTaken
                    });
                }

                _dbContext.SubmittedOption.AddRange(submittedOptions);
            }

            var submittedTest = await _dbContext.SubmittedTest.FindAsync(request.SubmittedTestId);
            submittedTest.TimeTaken = request.TimeTaken;
            _dbContext.SubmittedTest.Update(submittedTest);

            await _dbContext.SaveChangesAsync();


            return new SaveSubmittedQuestionResponseModel { Success = true };
        }
    }
}
