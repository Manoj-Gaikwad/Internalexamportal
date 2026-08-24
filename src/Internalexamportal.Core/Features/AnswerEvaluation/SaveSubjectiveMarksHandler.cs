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
    public class SaveSubjectiveMarksRequestModel : IRequest<SaveSubjectiveMarksResponseModel>
    {
        public int Id { get; set; }
        public ICollection<SubjectiveMarks> SubmittedAnswers { get; set; }

    }

    public class SubjectiveMarks
    {
        public int QuestionId { get; set; }
        public int Marks { get; set; }
        public string Comment { get; set; }
    }

    public class SaveSubjectiveMarksResponseModel
    {
        public bool Success { get; set; }
    }

    public class SaveSubjectiveMarksHandler : IRequestHandler<SaveSubjectiveMarksRequestModel, SaveSubjectiveMarksResponseModel>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPrincipal _principle;

        public SaveSubjectiveMarksHandler(InternalExamportalContext dbContext, IMapper mapper, IPrincipal principal)
        {
            _principle = principal;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<SaveSubjectiveMarksResponseModel> Handle(SaveSubjectiveMarksRequestModel request, CancellationToken cancellationToken)
        {
            var submittedTest = await _dbContext.SubmittedTest.FindAsync(request.Id);

            submittedTest.StatusId = (int)TestStatusEnum.Evaluated;

            List<SubjectiveAnswer> subjectiveAnswers = await _dbContext.SubjectiveAnswer
                                                            .Where(prop => prop.SubmittedTestId == request.Id)
                                                            .ToListAsync();

            foreach (var answer in subjectiveAnswers)
            {
                var updatedMarks = request.SubmittedAnswers.Where(prop => prop.QuestionId == answer.QuestionId).FirstOrDefault();
                if (updatedMarks != null)
                {
                    answer.ObtainedMarks = updatedMarks.Marks;
                    answer.Comment = updatedMarks.Comment;

                    if (updatedMarks.Marks > 0)
                    {
                        submittedTest.RightMark += updatedMarks.Marks;
                        submittedTest.CorrectQuestion++;
                    }
                    else if (updatedMarks.Marks < 0)
                    {
                        submittedTest.NegativeMark += (updatedMarks.Marks * -1);
                        submittedTest.InCorrectQuestion++;
                    }
                }
            }

            submittedTest.TotalMark = submittedTest.ObtainedMarks;

            _dbContext.SubmittedTest.Update(submittedTest);
            _dbContext.SubjectiveAnswer.UpdateRange(subjectiveAnswers);
            _dbContext.SaveChanges();

            return new SaveSubjectiveMarksResponseModel { Success = true };
        }
    }
}
