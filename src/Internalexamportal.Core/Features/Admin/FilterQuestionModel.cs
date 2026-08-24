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
    public class FilterQuestionModel : IRequest<ICollection<FilterQuestionResult>>
    {
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int SubjectTopicId { get; set; }
        public SubjectTopic SubjectTopic { get; set; }
        public int QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }
    }
    public class FilterQuestionResult
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int SubjectTopicId { get; set; }
        public SubjectTopic SubjectTopic { get; set; }
        public int QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }
        public QuestionMark QuestionMark { get; set; }

    }
    public class FilterQuestionHandler : IRequestHandler<FilterQuestionModel, ICollection<FilterQuestionResult>>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public FilterQuestionHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;

        }

        public async Task<ICollection<FilterQuestionResult>> Handle(FilterQuestionModel request, CancellationToken cancellationToken)
        {
            var data = await _dbContext.Question.Where(prop => prop.SubjectId == request.SubjectId
                                 && prop.SubjectTopicId == request.SubjectTopicId
                                 && prop.QuestionTypeId == request.QuestionTypeId)
                                .Include(prop => prop.QuestionMark)
                                .Include(prop => prop.Subject)
                                .Include(prop => prop.SubjectTopic)
                                .Include(pro => pro.QuestionType).ToListAsync();
            var mapped= _mapper.Map<List<FilterQuestionResult>>(data);

            return mapped;
        }
    }
}
