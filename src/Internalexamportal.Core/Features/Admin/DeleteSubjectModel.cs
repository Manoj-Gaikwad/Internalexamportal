using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features
{
    public class DeleteSubjectModel : IRequest<DeleteSubjectResult>
    {
        public int Id { get; set; }
    }
    public class DeleteSubjectResult
    {

    }
    public class DeleteSubjectHandler : IRequestHandler<DeleteSubjectModel, DeleteSubjectResult>
    {
        private readonly InternalExamportalContext _dbContext;
        public DeleteSubjectHandler(InternalExamportalContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DeleteSubjectResult> Handle(DeleteSubjectModel model, CancellationToken token)
        {
            var subjectId = _dbContext.Subject.Find(model.Id);
            if (subjectId != null)
            {
                _dbContext.Subject.Remove(subjectId);
                _dbContext.SaveChanges();
            }
            return new DeleteSubjectResult();
        }


    }
}
