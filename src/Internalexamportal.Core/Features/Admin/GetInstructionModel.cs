using AutoMapper;
using Internalexamportal.Core.Services;
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
   public class GetInstructionModel : IRequest<ICollection<GetInstructionModelResult>>
    {
        public int Id { get; set; }
    }
    public class GetInstructionModelResult
    {
        public int Id { get; set; }
        public string Instruction { get; set; }
        public string InstructionDescripiton { get; set; }
    }

    public class GetDifficultLevelHandler : IRequestHandler<GetInstructionModel, ICollection<GetInstructionModelResult>>
    {
        private readonly IRepository<TestInstruction> _instruction;
        private readonly IMapper _mapper;
        private readonly IClientManagerService _clientManager;
        public GetDifficultLevelHandler(IRepository<TestInstruction> instruction,
            IMapper mapper,
            IClientManagerService clientManager)
        {
            _instruction = instruction;
            _mapper = mapper;
            _clientManager = clientManager;
        }

        public async Task<ICollection<GetInstructionModelResult>> Handle(
     GetInstructionModel request,
     CancellationToken cancellationToken)
        {
            var client = await _clientManager.GetClientId();

            var instructions = await _instruction.FindAll()
                .Where(prop => client.Item2 || client.Item1 == prop.ClientId)
                .ToListAsync(cancellationToken);

            var mapped = _mapper.Map<List<GetInstructionModelResult>>(instructions);
            return mapped; // List<T> implements ICollection<T>
        }

    }
}
