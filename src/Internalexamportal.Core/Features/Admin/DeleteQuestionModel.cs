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
    public class DeleteQuestionModel : IRequest<DeleteQuestionResult>
    {
        public string Id { get; set; }
    }
    public class DeleteQuestionResult
    {

    }
    public class DeleteQuestionHandler : IRequestHandler<DeleteQuestionModel, DeleteQuestionResult>
    {

        private readonly IMapper _mapper;
        private readonly InternalExamportalContext _dbContext;
        public DeleteQuestionHandler(IMapper mapper, InternalExamportalContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;

        }

        public async Task<DeleteQuestionResult> Handle(DeleteQuestionModel model, CancellationToken token)
        {
            var id = int.Parse(model.Id);
            var questionId = _dbContext.Question.Find(id);
            var optionId = _dbContext.QuestionOption.Where(prop => prop.QuestionId == questionId.Id);
            var answerId = _dbContext.QuestionAnswer.Where(prop => prop.QuestionId == questionId.Id);
            var questionMark = _dbContext.QuestionMark.Where(prop => prop.QuestionId == questionId.Id);

            if (questionId != null)
            {
                _dbContext.Question.Remove(questionId);
            }
            if (questionMark != null)
            {
                _dbContext.QuestionMark.RemoveRange(questionMark);
            }
            if (optionId != null)
            {
                _dbContext.QuestionOption.RemoveRange(optionId);
            }
            if (answerId != null)
            {
                _dbContext.QuestionAnswer.RemoveRange(answerId);
            }

            _dbContext.SaveChanges();

            return new DeleteQuestionResult();
        }

    }
}
