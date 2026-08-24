using AutoMapper;
using Internalexamportal.DataAccessLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin.testManagement
{
    public class GetFilteredQuestionForTestModel : IRequest<List<FilteredQuestionModel>>
    {
        public int TestId { get; set; }
        public int SubjectId { get; set; }
        public int SubjectTopicId { get; set; }
        public int QuestionTypeId { get; set; }
    }

    public class FilteredQuestionModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int PositiveMark { get; set; }
        public int NegativeMark { get; set; }
    }
    public class GetFilteredQuestionForTestHandler : IRequestHandler<GetFilteredQuestionForTestModel, List<FilteredQuestionModel>>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetFilteredQuestionForTestHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;

        }

        public async Task<List<FilteredQuestionModel>> Handle(GetFilteredQuestionForTestModel request, CancellationToken cancellationToken)
        {
            var testQuestionIds = _dbContext.TestQuestion
                                            .Where(prop => prop.TestId == request.TestId)
                                            .Select(prop => prop.QuestionId)
                                            .ToList();

            var questions = await _dbContext.Question
                                      .Where(prop => prop.SubjectId == request.SubjectId
                                             && prop.SubjectTopicId == request.SubjectTopicId
                                             && prop.QuestionTypeId == request.QuestionTypeId
                                             && !testQuestionIds.Contains(prop.Id))
                                        .Include(prop => prop.QuestionMark)
                                        .ToListAsync();

            var result = _mapper.Map<List<FilteredQuestionModel>>(questions);


            return result;
        }
    }
}
