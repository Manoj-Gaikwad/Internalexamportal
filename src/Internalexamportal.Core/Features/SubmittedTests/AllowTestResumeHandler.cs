using AutoMapper;
using Internalexamportal.DataAccessLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.SubmittedTests
{
    public class AllowTestResumeRequestModel : IRequest<AllowTestResumeResponseModel>
    {
        public int SubmittedTestId { get; set; }
    }
    public class AllowTestResumeResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class AllowTestResumeHandler : IRequestHandler<AllowTestResumeRequestModel, AllowTestResumeResponseModel>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public AllowTestResumeHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<AllowTestResumeResponseModel> Handle(AllowTestResumeRequestModel request, CancellationToken cancellationToken)
        {
            var submittedTest = await _dbContext.SubmittedTest.FindAsync(request.SubmittedTestId);

            submittedTest.AllowTestResume = !submittedTest.AllowTestResume;

            _dbContext.SubmittedTest.Update(submittedTest);
            await _dbContext.SaveChangesAsync();

            return new AllowTestResumeResponseModel { Success = true };
        }


    }
}
