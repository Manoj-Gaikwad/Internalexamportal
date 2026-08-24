using AutoMapper;
using Internalexamportal.Core.Features.Candidate;
using Internalexamportal.DataAccessLayer;
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
  public  class GetTopicModel:IRequest<ICollection<GetSubjectTopicResult>>
    {
        public int SubjectId { get; set; }
    }
    public class GetSubjectTopicResult
    {
        public int Id { get; set; }
        public String Topic { get; set; }
        public int SubjectId { get; set; }

        public ICollection<QuestionDto> QuestionsData { get; set; }
    }
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class GetTopicHandler : IRequestHandler<GetTopicModel, ICollection<GetSubjectTopicResult>>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetTopicHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ICollection<GetSubjectTopicResult>> Handle(GetTopicModel request, CancellationToken cancellationToken)
        {
             var subjectTopics = await _dbContext.SubjectTopic
            .Include(st => st.Questions) 
            .Where(st => st.SubjectId == request.SubjectId)
            .ToListAsync(cancellationToken);

            var mapped = _mapper.Map<ICollection<GetSubjectTopicResult>>(subjectTopics);
            return mapped;
        }
    }
}
