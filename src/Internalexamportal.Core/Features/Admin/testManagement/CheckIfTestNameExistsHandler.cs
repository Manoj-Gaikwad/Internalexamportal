using Internalexamportal.DataAccessLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin.testManagement
{
    public class CheckIfTestNameExistsRequest : IRequest<CheckIfTestNameExistsResponse>
    {
        public int Id { get; set; }
        public string TestName { get; set; }
    }

    public class CheckIfTestNameExistsResponse
    {
        public bool IsExists { get; set; }
    }

    public class CheckIfTestNameExistsHandler : IRequestHandler<CheckIfTestNameExistsRequest, CheckIfTestNameExistsResponse>
    {
        private readonly InternalExamportalContext _dbContext;

        public CheckIfTestNameExistsHandler(InternalExamportalContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CheckIfTestNameExistsResponse> Handle(CheckIfTestNameExistsRequest message, CancellationToken token)
        {
            //Find the name
            var exists = await _dbContext.Test.AnyAsync(prop => prop.Id != message.Id 
                                                    && prop.TestName == message.TestName);

            return new CheckIfTestNameExistsResponse { IsExists = exists };
        }
    }
}
