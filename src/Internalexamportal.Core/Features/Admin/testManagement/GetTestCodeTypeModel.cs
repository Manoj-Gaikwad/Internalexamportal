using AutoMapper;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin.testManagement
{
   public class GetTestCodeTypeModel : IRequest<ICollection<GetTestCodeTypeResult>>
    {
        
    }
    public class GetTestCodeTypeResult
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
    public class GetTestCodeTypeHandler : IRequestHandler<GetTestCodeTypeModel, ICollection<GetTestCodeTypeResult>>
    {
        private readonly IRepository<ActivationType> _testType;
        private readonly IMapper _mapper;
        public GetTestCodeTypeHandler(IRepository<ActivationType> testType, IMapper mapper)
        {
            _testType = testType;
            _mapper = mapper;
        }

        public async Task<ICollection<GetTestCodeTypeResult>> Handle(GetTestCodeTypeModel request, CancellationToken cancellationToken)
        {
            // Make sure FindAll is awaited if it's async. 
            // If not async, still call ToList() on it.
            var activationTypes = _testType.FindAll().ToList();

            var mapped = _mapper.Map<ICollection<GetTestCodeTypeResult>>(activationTypes);

            return await Task.FromResult(mapped);
        }



    }
}
