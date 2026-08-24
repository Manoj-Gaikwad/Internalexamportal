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

namespace Internalexamportal.Core.Features
{
   public class GetQuestionTypeModel : IRequest<ICollection<GetQuestionTypeResult>>
    {
    }

    public class GetQuestionTypeResult
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }


    public class GetQuestionTypeHandler : IRequestHandler<GetQuestionTypeModel, ICollection<GetQuestionTypeResult>>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetQuestionTypeHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ICollection<GetQuestionTypeResult>> Handle(GetQuestionTypeModel request, CancellationToken cancellationToken)
        {
            var questionType = await _dbContext.QuestionType.Where(prop => prop.Id != 4).ToListAsync();
            var mapped = _mapper.Map<List<GetQuestionTypeResult>>(questionType);
            return mapped;
        }

    }
}
