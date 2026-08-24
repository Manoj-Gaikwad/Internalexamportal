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
   public class UpdateTestPublicModel:IRequest<UpdateTestPublicResult>
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int TestId { get; set; }
    }
    public class UpdateTestPublicResult
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int TestId { get; set; }
    }
    public class UpdateTestPublicHandler : IRequestHandler<UpdateTestPublicModel, UpdateTestPublicResult>
    {
        private readonly IRepository<TestPublish> _instruction;
        private readonly IMapper _mapper;

        public UpdateTestPublicHandler(IRepository<TestPublish> instruction, IMapper mapper)
        {
            _instruction = instruction;
            _mapper = mapper;
        }
        public async Task<UpdateTestPublicResult> Handle(UpdateTestPublicModel request, CancellationToken cancellationToken)
        {
            var result = new UpdateTestPublicResult();
            var testPublish = _mapper.Map<TestPublish>(request);
            _instruction.Update(testPublish);
            _instruction.SaveChanges();
            return result;
        }
    }
}
