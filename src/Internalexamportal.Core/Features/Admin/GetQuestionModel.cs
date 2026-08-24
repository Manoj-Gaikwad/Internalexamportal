using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Contracts;
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
  public class GetQuestionModel : IRequest<ICollection<GetQuestionResult>>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int SubjectId { get; set; }
        public int QuestionTypeId { get; set; }
       
    }
    public class GetQuestionResult
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int SubjectTopicId { get; set; }
        public SubjectTopic SubjectTopic { get; set; }
        public int QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }

       
    }

    public class GetQuestionHandler : IRequestHandler<GetQuestionModel, ICollection<GetQuestionResult>>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetQuestionHandler(IMapper mapper, InternalExamportalContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ICollection<GetQuestionResult>> Handle(GetQuestionModel request, CancellationToken cancellationToken)
        {
            var questions = await _dbContext.Question.Include(prop=>prop.Subject).Include( prop=>prop.SubjectTopic).ToListAsync(cancellationToken);
            var mapped = _mapper.Map<List<GetQuestionResult>>(questions);
            return mapped;
        }
    }

}
