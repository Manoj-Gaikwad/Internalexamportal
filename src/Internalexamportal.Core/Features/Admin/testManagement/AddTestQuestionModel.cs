using AutoMapper;
using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin.testManagement
{
    public class AddTestQuestionModel : IRequest<AddTestQuestionResult>
    {
        public int TestId { get; set; }
        public List<TestQuestionRequestModel> Questions { get; set; }
    }

    public class TestQuestionRequestModel
    {
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public int PositiveMark { get; set; }
        public int NegativeMark { get; set; }
    }

    public class AddTestQuestionResult
    {
    }

    public class AddTestQuestionHandler : IRequestHandler<AddTestQuestionModel, AddTestQuestionResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public AddTestQuestionHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<AddTestQuestionResult> Handle(AddTestQuestionModel request, CancellationToken cancellationToken)
        {
            var testQuestionsIds = await _dbContext.TestQuestion
                                            .Where(prop => prop.TestId == request.TestId)
                                            .Select(prop => prop.QuestionId)
                                            .ToArrayAsync();

            request.Questions = request.Questions.Where(prop => !testQuestionsIds.Contains(prop.QuestionId)).ToList();

            var testQuestions = _mapper.Map<List<TestQuestion>>(request.Questions);

            await _dbContext.TestQuestion.AddRangeAsync(testQuestions);
            await _dbContext.SaveChangesAsync();

            return new AddTestQuestionResult();
        }
    }
}