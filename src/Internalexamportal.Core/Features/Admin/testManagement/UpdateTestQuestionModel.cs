using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin.testManagement
{
    public class UpdateTestQuestionModel : IRequest<UpdateTestQuestionResult>
    {
        public int Id { get; set; }
        public int NegativeMark { get; set; }
        public int PositiveMark { get; set; }
    }
    public class UpdateTestQuestionResult
    {
        public bool Success { get; set; }
    }
    public class UpdateTestQuestionHandler : IRequestHandler<UpdateTestQuestionModel, UpdateTestQuestionResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateTestQuestionHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<UpdateTestQuestionResult> Handle(UpdateTestQuestionModel request, CancellationToken cancellationToken)
        {
            var result = new UpdateTestQuestionResult();
            var testQuestion = await _dbContext.TestQuestion.FindAsync(request.Id);

            if(testQuestion != null)
            {
                testQuestion.PositiveMark = request.PositiveMark;
                testQuestion.NegativeMark = request.NegativeMark;
                _dbContext.TestQuestion.Update(testQuestion);
                await _dbContext.SaveChangesAsync();
                result.Success = true;
            }

            return result;
        }
    }
}