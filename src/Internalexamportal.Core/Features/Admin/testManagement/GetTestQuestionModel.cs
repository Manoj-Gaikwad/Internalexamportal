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

namespace Internalexamportal.Core.Features.Admin.testManagement
{
    public class GetTestQuestionModel : IRequest<List<TestQuestionModel>>
    {
        public int Id { get; set; }
    }

    public class TestQuestionModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Description { get; set; }
        public int PositiveMark { get; set; }
        public int NegativeMark { get; set; }
    }

    public class GetTestQuestionHandler : IRequestHandler<GetTestQuestionModel, List<TestQuestionModel>>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public GetTestQuestionHandler(InternalExamportalContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<TestQuestionModel>> Handle(GetTestQuestionModel request, CancellationToken cancellationToken)
        {

            var testQuestions = await _dbContext.TestQuestion
                                    .Where(a => a.TestId == request.Id)
                                    .Include(prop => prop.Question)
                                        .ThenInclude(q => q.QuestionMark)
                                    .ToListAsync();

            var result = _mapper.Map<List<TestQuestionModel>>(testQuestions);

            return result;

        }


    }

}
