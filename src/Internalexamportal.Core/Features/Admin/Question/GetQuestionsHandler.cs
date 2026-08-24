using AutoMapper;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin
{
    public class GetQuestionsModel : IRequest<GetQuestionsResult>
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string SearchTerm { get; set; }
        public string SortingColumn { get; set; }
        public string SortingDirection { get; set; }
        public int[] SubjectIds { get; set; }
        public int[] SubjectTopicIds { get; set; }
        public int[] QuestionTypeIds { get; set; }
    }

    public class GetQuestionsResult
    {
        public List<QuestionResultModel> Questions { get; set; }
        public int TotalCount { get; set; }
    }

    public class QuestionResultModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }
        public string Type { get; set; }
        public QuestionMark QuestionMark { get; set; }
    }

    public class GetQuestionsHandler : IRequestHandler<GetQuestionsModel, GetQuestionsResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IClientManagerService _clientManager;

        public GetQuestionsHandler(
            InternalExamportalContext dbContext,
            IMapper mapper,
            IClientManagerService clientManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _clientManager = clientManager;
        }

        public async Task<GetQuestionsResult> Handle(GetQuestionsModel model, CancellationToken cancellationToken)
        {
            var client = await _clientManager.GetClientId();

            // Base query
            var query = _dbContext.Question
                .Include(q => q.QuestionMark)
                .Include(q => q.Subject)
                .Include(q => q.SubjectTopic)
                .Include(q => q.QuestionType)
                .Where(q => model.SubjectIds == null || !model.SubjectIds.Any() || model.SubjectIds.Contains(q.SubjectId))
                .Where(q => model.SubjectTopicIds == null || !model.SubjectTopicIds.Any() || model.SubjectTopicIds.Contains(q.SubjectTopicId))
                .Where(q => model.QuestionTypeIds == null || !model.QuestionTypeIds.Any() || model.QuestionTypeIds.Contains(q.QuestionTypeId))
                .Where(q => client.Item2 || client.Item1 == q.ClientId)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                var search = model.SearchTerm.Trim().ToLower();
                query = query.Where(q => q.Description.ToLower().Contains(search));
            }

            // Sorting
            query = model.SortingColumn?.ToLower() switch
            {
                "description" => model.SortingDirection?.ToLower() == "desc" ? query.OrderByDescending(q => q.Description) : query.OrderBy(q => q.Description),
                "topic" => model.SortingDirection?.ToLower() == "desc" ? query.OrderByDescending(q => q.SubjectTopic.Topic) : query.OrderBy(q => q.SubjectTopic.Topic),
                "subject" => model.SortingDirection?.ToLower() == "desc" ? query.OrderByDescending(q => q.Subject.SubjectName) : query.OrderBy(q => q.Subject.SubjectName),
                "type" => model.SortingDirection?.ToLower() == "desc" ? query.OrderByDescending(q => q.QuestionType.Type) : query.OrderBy(q => q.QuestionType.Type),
                _ => query.OrderBy(q => q.Id) // default sort
            };

            // Total count before pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Pagination
            var questionsPaged = await query
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToListAsync(cancellationToken);

            // Map to DTO
            var questionsResult = _mapper.Map<List<QuestionResultModel>>(questionsPaged);

            return new GetQuestionsResult
            {
                Questions = questionsResult,
                TotalCount = totalCount
            };
        }
    }
}
