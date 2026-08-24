using Internalexamportal.DataAccessLayer;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Candidate.Groups
{

    public class DeleteGroupModel: IRequest<DeleteGroupResult>
    {
        public int Id { get; set; }
    }

    public class DeleteGroupResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class DeleteGroupHandler:IRequestHandler<DeleteGroupModel,DeleteGroupResult>
    {
        private readonly InternalExamportalContext _dbContext;

        public DeleteGroupHandler(InternalExamportalContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DeleteGroupResult> Handle(DeleteGroupModel request,CancellationToken token)
        {
            var result = new DeleteGroupResult();
            var group= await _dbContext.Group.FindAsync(request.Id);

            if(group == null)
            {
                result.Success = false;
                result.Message = "Group not found";
                return result;
            }
            else
            {
                group.IsDeleted = true;
                _dbContext.Group.Update(group);
                await _dbContext.SaveChangesAsync();
                result.Success = true;
                result.Message = "Group deleted successfully";
                return result;
            }
        }

    }
}
