using AutoMapper;
using Internalexamportal.DataAccessLayer;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Principal;
using Internalexamportal.Common.Extensions;
using InternalExamportal.DataAccessLayer.Entities;
using Internalexamportal.Core.Commons.Utils;
using Internalexamportal.Core.Enums;

namespace Internalexamportal.Core.Features.Candidate
{
    public class StartTestRequestModel : IRequest<StartTestResponseModel>
    {
        public string UserId { get; set; }
        public int TestId { get; set; }

    }

    public class StartTestResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool IsPendingTest { get; set; }
        public int SubmittedTestId { get; set; }
        public double TimeTaken { get; set; }
    }

    public class StartTestHandler : IRequestHandler<StartTestRequestModel, StartTestResponseModel>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPrincipal _principle;

        public StartTestHandler(InternalExamportalContext dbContext, IMapper mapper, IPrincipal principal)
        {
            _principle = principal;
            _dbContext = dbContext;
            _mapper = mapper;

        }

        public async Task<StartTestResponseModel> Handle(StartTestRequestModel request, CancellationToken cancellationToken)
        {
            var result = new StartTestResponseModel();

            request.UserId = _principle.Identity.GetUserId();

            var test = _dbContext.Test
                .Where(prop => prop.Id == request.TestId)
                .FirstOrDefault();

            var PendingTest = await _dbContext.SubmittedTest
                                                .Where(prop => prop.TestId == request.TestId
                                                               && prop.UserId == request.UserId
                                                               && prop.StatusId == (int)TestStatusEnum.InProgress)
                                                .FirstOrDefaultAsync();

            if (PendingTest != null)
            {
                if (!PendingTest.AllowTestResume)
                {
                    result.Success = false;
                    result.Message = @"Same test is in progress for current user please submit that test.
                                    If you are unable to submit the test please contact admin.";
                    return result;
                }
                else
                {
                    result.Success = true;
                    result.SubmittedTestId = PendingTest.Id;
                    result.IsPendingTest = true;
                    result.TimeTaken = PendingTest.TimeTaken;
                    PendingTest.AllowTestResume = false;
                    _dbContext.SubmittedTest.Update(PendingTest);
                    await _dbContext.SaveChangesAsync();
                    return result;
                }
            }

            SubmittedTest submittedTest = new SubmittedTest
            {
                MaximumMark = test.TotalMark,
                UserId = request.UserId,
                TestId = request.TestId,
                SubmitDate = Utilities.GetISTDateTime(),
                StatusId = (int)TestStatusEnum.InProgress
            };

            await _dbContext.SubmittedTest.AddAsync(submittedTest);
            await _dbContext.SaveChangesAsync();

            return new StartTestResponseModel { Success = true,SubmittedTestId = submittedTest.Id };
        }
    }
}
