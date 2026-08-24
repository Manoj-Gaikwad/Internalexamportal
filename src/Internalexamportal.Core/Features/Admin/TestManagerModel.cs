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
    public class TestManagerModel : IRequest<TestManagerResult>
    {


        public string TestName { get; set; }
        public double Duration { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalMark { get; set; }
        public int QuestionTypeId { get; set; }
        public int TestInstructionId { get; set; }
        public int DifficultLevelId { get; set; }
    }

    public class TestManagerResult
    {
        public int Id { get; set; }
    }

    public class CreateTestHandler : IRequestHandler<TestManagerModel, TestManagerResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public CreateTestHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<TestManagerResult> Handle(TestManagerModel request, CancellationToken cancellationToken)
        {
            var tests = _mapper.Map<Test>(request);
           await _dbContext.Test.AddAsync(tests);
           await _dbContext.SaveChangesAsync();
            var data = _dbContext.Test.Find(tests.Id);
            return new TestManagerResult { Id = data.Id };
        }
    }
}