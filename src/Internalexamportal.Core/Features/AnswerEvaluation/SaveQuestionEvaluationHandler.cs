using AutoMapper;
using Internalexamportal.Core.Enums;
using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.AnswerEvaluation
{
    public class SaveQuestionEvaluationRequestModel : IRequest<SaveQuestionEvaluationResponseModel>
    {
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public int Marks { get; set; }
        public string Comment { get; set; }

    }

    public class SaveQuestionEvaluationResponseModel
    {
        public bool Success { get; set; }
    }

    public class SaveQuestionEvaluationHandler : IRequestHandler<SaveQuestionEvaluationRequestModel, SaveQuestionEvaluationResponseModel>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPrincipal _principle;

        public SaveQuestionEvaluationHandler(InternalExamportalContext dbContext, IMapper mapper, IPrincipal principal)
        {
            _principle = principal;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<SaveQuestionEvaluationResponseModel> Handle(SaveQuestionEvaluationRequestModel request, CancellationToken cancellationToken)
        {
            var submittedTest = await _dbContext.SubmittedTest.FindAsync(request.TestId);

            SubjectiveAnswer subjectiveAnswer = await _dbContext.SubjectiveAnswer
                                                            .Where(prop => prop.SubmittedTestId == request.TestId)
                                                            .Where(prop => prop.QuestionId == request.QuestionId)
                                                            .FirstOrDefaultAsync();

            if (subjectiveAnswer != null)
            {
                subjectiveAnswer.ObtainedMarks = request.Marks;
                subjectiveAnswer.Comment = request.Comment;

                _dbContext.SubjectiveAnswer.Update(subjectiveAnswer);
                _dbContext.SaveChanges();
            }

            return new SaveQuestionEvaluationResponseModel { Success = true };
        }
    }
}
