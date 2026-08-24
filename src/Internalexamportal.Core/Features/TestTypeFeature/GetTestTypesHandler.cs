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

namespace Internalexamportal.Core.Features.Admin
{
    public class GetTestTypesModel : IRequest<ICollection<GetTestTypesResult>>
    {
    }

    public class GetTestTypesResult
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }


    public class GetTestTypesHandler : IRequestHandler<GetTestTypesModel, ICollection<GetTestTypesResult>>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetTestTypesHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ICollection<GetTestTypesResult>> Handle(GetTestTypesModel request, CancellationToken cancellationToken)
        {
            var questionType = await _dbContext.TestType.ToListAsync();
            return _mapper.Map<ICollection<GetTestTypesResult>>(questionType);
        }
    }
}
