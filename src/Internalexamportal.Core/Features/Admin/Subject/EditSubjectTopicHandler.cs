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
     public class EditSubjectTopicModel : IRequest<EditSubjectTopicResult>
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public int SubjectId { get; set; }
    }

    public class EditSubjectTopicResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class CreateEditSubjectTopicHandler : IRequestHandler<EditSubjectTopicModel, EditSubjectTopicResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
    
        public CreateEditSubjectTopicHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<EditSubjectTopicResult> Handle(EditSubjectTopicModel request, CancellationToken cancellationToken)
        {
            var result = new EditSubjectTopicResult();

            var topic =await _dbContext.SubjectTopic.FindAsync(request.Id);
            topic.Topic = request.Topic;


            bool isExists = await _dbContext.SubjectTopic
                                    .AnyAsync(prop => prop.Topic == topic.Topic
                                                    && prop.SubjectId == topic.SubjectId
                                                    && prop.Id != topic.Id);

            if (isExists)
            {
                result.Success = false;
                result.Message = "Subject Topic Already Exists";
                return result;
            }

            _dbContext.Update(topic);
            await _dbContext.SaveChangesAsync();
            result.Success = true;

            return result;
        }
    }
}
