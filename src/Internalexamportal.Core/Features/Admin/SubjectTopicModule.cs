using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features
{
    public class SubjectTopicModule : IRequest<SubjectTopicResult>
    {
        public int Id { get; set; }
        public String Topic { get; set; }
        public int SubjectId { get; set; }
    }

    public class SubjectTopicResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class CreateSubjectTopicHandler : IRequestHandler<SubjectTopicModule, SubjectTopicResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public CreateSubjectTopicHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<SubjectTopicResult> Handle(SubjectTopicModule request, CancellationToken cancellationToken)
        {
            var result = new SubjectTopicResult();

            var subjectTopic = _mapper.Map<SubjectTopic>(request);

            bool isExists = await _dbContext.SubjectTopic
                    .AnyAsync(prop => prop.Topic == subjectTopic.Topic
                                    && prop.SubjectId == subjectTopic.SubjectId);

            if (isExists)
            {
                result.Success = false;
                result.Message = "Subject Topic Already Exists";
                return result;
            }

            _dbContext.SubjectTopic.Add(subjectTopic);
            _dbContext.SaveChanges();
            result.Success = true;
            return result; ;
        }
    }
}
