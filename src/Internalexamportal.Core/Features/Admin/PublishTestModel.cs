using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin
{
   public class PublishTestModel : IRequest<PublishTestResult>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int TestId { get; set; }
    }
    public class PublishTestResult
    {

    }
    public class PublishTestHandler : IRequestHandler<PublishTestModel, PublishTestResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public PublishTestHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<PublishTestResult> Handle(PublishTestModel request, CancellationToken cancellationToken)
        {
            var testInstruction = _mapper.Map<TestPublish>(request);
           await _dbContext.TestPublish.AddAsync(testInstruction);
          await  _dbContext.SaveChangesAsync();
            return new PublishTestResult();
        }
    }
}
