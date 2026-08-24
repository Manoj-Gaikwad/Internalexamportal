using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Candidate
{
    public class GetUpcomingTestsRequestModel : IRequest<GetUpcomingTestsResponseModel>
    {
    }

    public class GetUpcomingTestsResponseModel
    {
        public List<Test> Tests { get; set; }
    }

    public class GetUpcomingTestsHandler : IRequestHandler<GetUpcomingTestsRequestModel, GetUpcomingTestsResponseModel>
    {

        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPrincipal _principle;

        public GetUpcomingTestsHandler(IMapper mapper, InternalExamportalContext dbContext, IPrincipal principle)
        {
            _dbContext = dbContext;
            _principle = principle;
            _mapper = mapper;
        }

        public async Task<GetUpcomingTestsResponseModel> Handle(GetUpcomingTestsRequestModel request, CancellationToken cancellationToken)
        {
            var examresult = new GetUpcomingTestsResponseModel();

            var user = _dbContext.Users.Find(_principle.Identity.GetUserId());


            var testIds = await _dbContext.SubmittedTest
                                        .Where(prop => prop.UserId == user.Id)
                                        .Select(prop => prop.TestId)
                                        .ToArrayAsync();


            var Ids = await _dbContext.AccessCode
                                        .Include(prop => prop.ActivationCode)
                                        .Where(prop => prop.Email == user.Email && !testIds.Contains(prop.ActivationCode.TestId))
                                        .Select(prop => prop.ActivationCode.TestId)
                                        .ToListAsync();

            examresult.Tests = await _dbContext.Test
                                                .Include(prop => prop.TestPublish)
                                                .Where(test => Ids.Contains(test.Id))
                                                .ToListAsync();


            foreach (var test in examresult.Tests)
            {
                if (test.TestPublish.StartDate.HasValue)
                {
                    test.TestPublish.StartDate = test.TestPublish.StartDate.Value;
                }

                if (test.TestPublish.EndDate.HasValue)
                {
                    test.TestPublish.EndDate = test.TestPublish.EndDate.Value;
                }
            }

            return examresult;
        }
    }
}
