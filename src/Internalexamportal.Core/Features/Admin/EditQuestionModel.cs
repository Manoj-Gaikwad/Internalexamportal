using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin
{
    public class EditQuestionModel : IRequest<EditQuestionResult>
    {
        public int Id { get; set; }
    }
    public class EditQuestionResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Question Question { get; set; }

    }

    public class EditQuestionResultHandler : IRequestHandler<EditQuestionModel, EditQuestionResult>
    {
        private readonly InternalExamportalContext _dbContext;

        public EditQuestionResultHandler(InternalExamportalContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<EditQuestionResult> Handle(EditQuestionModel request, CancellationToken cancellationToken)
        {
            var question = _dbContext.Question
                .Where(b => b.Id == request.Id)
                .Include(p => p.QuestionMark)
                .Include(p => p.QuestionOption)
                .FirstOrDefault();
            return new EditQuestionResult { success = (question != null),message = "Success",Question =question};
        }
    }
}