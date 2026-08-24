using AutoMapper;
using Internalexamportal.Core.Services;
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

namespace Internalexamportal.Core.Features.Admin
{
   public class TestInstructionModel : IRequest<TestInstructionResult>
    {
        public int Id { get; set; }
        public string Instruction { get; set; }
        public string InstructionDescripiton { get; set; }
    }
    public class TestInstructionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class TestInstructionHandler : IRequestHandler<TestInstructionModel, TestInstructionResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IClientManagerService _clientManager;

        public TestInstructionHandler(InternalExamportalContext dbContext,
            IMapper mapper,
            IClientManagerService clientManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _clientManager = clientManager;
        }
        public async Task<TestInstructionResult> Handle(TestInstructionModel request, CancellationToken cancellationToken)
        {
            var result = new TestInstructionResult();

            var testInstruction = _mapper.Map<TestInstruction>(request);

            var client = await _clientManager.GetClientId();
            testInstruction.ClientId = client.Item1;

            if (testInstruction.Id == 0)
            {
                bool isExists = await _dbContext.TestInstruction
                                        .AnyAsync(prop => prop.Instruction == testInstruction.Instruction
                                                        && prop.ClientId == testInstruction.ClientId);
                if (isExists)
                {
                    result.Success = false;
                    result.Message = "Instruction with same name already exists";
                    return result;
                }
                await _dbContext.TestInstruction.AddAsync(testInstruction);
            }
            else
            {
                var oldInstructions = await _dbContext.TestInstruction.AsNoTracking()
                                                      .Where(prop => prop.Id == testInstruction.Id)
                                                      .FirstOrDefaultAsync();

                testInstruction.ClientId = oldInstructions.ClientId;

                bool isExists = await _dbContext.TestInstruction
                                        .AnyAsync(prop => prop.Instruction == testInstruction.Instruction
                                                        && prop.ClientId == testInstruction.ClientId
                                                        && prop.Id != testInstruction.Id);

                if (isExists)
                {
                    result.Success = false;
                    result.Message = "Instruction with same name already exists";
                    return result;
                }

                _dbContext.TestInstruction.Update(testInstruction);

            }

            await _dbContext.SaveChangesAsync();
            result.Success = true;
            return result;
        }
    }
}
