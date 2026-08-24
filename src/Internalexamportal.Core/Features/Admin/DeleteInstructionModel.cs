using AutoMapper;
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
    public class DeleteInstructionModel:IRequest<DeleteInstructionResult>
    {
        public string Id { get; set; }
    }
    public class DeleteInstructionResult
    {

    }
    public class DeleteInstructionHandler : IRequestHandler<DeleteInstructionModel, DeleteInstructionResult>
    {

        private readonly IRepository<TestInstruction> _instruction;
        private readonly IMapper _mapper;
        public DeleteInstructionHandler(IRepository<TestInstruction> instruction, IMapper mapper)
        {
            _instruction = instruction;
            _mapper = mapper;

        }

    
        public async Task<DeleteInstructionResult> Handle(DeleteInstructionModel model, CancellationToken cancellationToken)
        {
            DeleteInstructionResult result = new DeleteInstructionResult();
            var id = int.Parse(model.Id);
            var instructionId = _instruction.Find(id);
            if (instructionId != null)
            {
                _instruction.Remove(instructionId);
                _instruction.SaveChanges();
            }
            return result;
        }
    }
}


