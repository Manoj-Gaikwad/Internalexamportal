using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin
{
    public class DeleteQuestionFromTestModel : IRequest<DeleteQuestionFromTestResult>
    {
        public int QuestionId { get; set; }
        public int TestId { get; set; }
    }
    public class DeleteQuestionFromTestResult
    {
        public string message { get; set; }
    }
    public class DeleteQuestionFromTestHandler : IRequestHandler<DeleteQuestionFromTestModel, DeleteQuestionFromTestResult>
    {

        private readonly InternalExamportalContext _dbContext;

        public DeleteQuestionFromTestHandler(InternalExamportalContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DeleteQuestionFromTestResult> Handle(DeleteQuestionFromTestModel request, CancellationToken token)
        {
            var testQuestionId = _dbContext.TestQuestion.Where(a => a.QuestionId == request.QuestionId & a.TestId == request.TestId).FirstOrDefault();

            if (testQuestionId != null)
            {
                _dbContext.TestQuestion.Remove(testQuestionId);
               _dbContext.SaveChanges();
            }

            return new DeleteQuestionFromTestResult { message = "OK" };
        }

    }
}
