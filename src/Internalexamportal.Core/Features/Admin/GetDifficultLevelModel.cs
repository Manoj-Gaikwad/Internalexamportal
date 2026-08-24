using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features
{
    public class GetDifficultLevelModel : IRequest<ICollection<GetDifficultLevelResult>>
    {
        public int Id { get; set; }
        public String Level { get; set; }
    }

    //Result
    public class GetDifficultLevelResult
    {
        public int Id { get; set; }
        public String Level { get; set; }
    }

    public class GetDifficultLevelHandler : IRequestHandler<GetDifficultLevelModel, ICollection<GetDifficultLevelResult>>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetDifficultLevelHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ICollection<GetDifficultLevelResult>> Handle(GetDifficultLevelModel request, CancellationToken cancellationToken)
        {
            var diffLevel = await _dbContext.DifficultLevel.ToListAsync(cancellationToken);

            var mapped=_mapper.Map<List<GetDifficultLevelResult>>(diffLevel);

            return mapped;
        }
    }

}
