using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Candidate
{
    public class GetTestInstructionModel : IRequest<InstructionTestResult>
    {
        public int Id { get; set; }
      
        //public List<TestInstruction> TestInstruction { get; set; }
    }

    public class InstructionTestResult
    {

        public int Id { get; set; }
        public string Instruction { get; set; }
        public string InstructionDescripiton { get; set; }
    }

    public class GetTestInstructionResult : IRequestHandler<GetTestInstructionModel, InstructionTestResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public GetTestInstructionResult(
            InternalExamportalContext dbContext, 
            IMapper mapper)
        {
            
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<InstructionTestResult> Handle(GetTestInstructionModel request, CancellationToken cancellationToken)
        {
        
             var TestInstruction = _dbContext.TestInstruction.Where(prop => prop.Id == request.Id).AsNoTracking().FirstOrDefault();
           return await Task.FromResult(_mapper.Map<InstructionTestResult>(TestInstruction)); 
        }
    }


}
