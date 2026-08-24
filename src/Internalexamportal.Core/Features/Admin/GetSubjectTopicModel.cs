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
    public class GetSubjectTopicModel : IRequest<ICollection<GetSubjectTopicResult>>
    {
    }
    public class GetSubjectTopicResult
    {
        public int Id { get; set; }
        public String SubjectTopic { get; set; }
    }

    public class GetSubjectTopicHandler : IRequestHandler<GetSubjectTopicModel, ICollection<GetSubjectTopicResult>>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetSubjectTopicHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ICollection<GetSubjectTopicResult>> Handle(GetSubjectTopicModel request, CancellationToken cancellationToken)
        {
            var subjectTopic = await _dbContext.SubjectTopic.ToListAsync(cancellationToken);

            var mapped = _mapper.Map<List<GetSubjectTopicResult>>(subjectTopic);
            return mapped;
        }
    }

}
