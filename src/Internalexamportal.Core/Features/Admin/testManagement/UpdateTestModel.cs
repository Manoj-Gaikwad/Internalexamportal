using AutoMapper;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin.testManagement
{
  public  class UpdateTestModel : IRequest<UpdateTestResult>
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public double Duration { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalMark { get; set; }
        public int QuestionTypeId { get; set; }
        public int TestInstructionId { get; set; }
        public int DifficultLevelId { get; set; }
       // public Guid LinkId { get; set; }
    }
    public class UpdateTestResult
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public double Duration { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalMark { get; set; }
        public int QuestionTypeId { get; set; }
        public int TestInstructionId { get; set; }
        public int DifficultLevelId { get; set; }
        public Guid LinkId { get; set; }
        public int TestId { get; set; }
    }
    public class UpdateTestHandler : IRequestHandler<UpdateTestModel, UpdateTestResult>
    {
        private readonly IRepository<Test> _test;
        private readonly IMapper _mapper;

        public UpdateTestHandler(IRepository<Test> test, IMapper mapper)
        {
            _test = test;
            _mapper = mapper;
        }

        public async Task<UpdateTestResult> Handle(UpdateTestModel request, CancellationToken cancellationToken)
        {
            var result = new UpdateTestResult();
            var tests = _mapper.Map<Test>(request);
            _test.Update(tests);
            _test.SaveChanges();
           
            return result;
        }
    }
}
