using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Enums;
using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Candidate
{
    public class GetCandidateTestsRequestModel : IRequest<GetCandidateTestsResponseModel>
    {
    }

    public class GetCandidateTestsResponseModel
    {
        public List<SubmittedTestModel> Tests { get; set; }
    }
    public class SubmittedTestModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string TestName { get; set; }
        public DateTime SubmitDate { get; set; }
        public int AttemptedQuestion { get; set; }
        public int SkippedQuestion { get; set; }
        public int ReviewedQuestion { get; set; }
        public int TotalQuestion { get; set; }
        public string TimeTaken { get; set; }
        public bool IsResultPublished { get; set; }
        public bool IsAnswerSheetAllowed { get; set; }
        public bool IsCertificateAllowed { get; set; }
        public int StatusId { get; set; }

    }
    public class GetCandidateTestsHandler : IRequestHandler<GetCandidateTestsRequestModel, GetCandidateTestsResponseModel>
    {

        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPrincipal _principle;

        public GetCandidateTestsHandler(IMapper mapper, InternalExamportalContext dbContext, IPrincipal principle)
        {
            _dbContext = dbContext;
            _principle = principle;
            _mapper = mapper;
        }

        public async Task<GetCandidateTestsResponseModel> Handle(GetCandidateTestsRequestModel request, CancellationToken cancellationToken)
        {
            var examresult = new GetCandidateTestsResponseModel();

            var UserId = _principle.Identity.GetUserId();

            var submittedTests = await _dbContext.SubmittedTest
                             .Where(prop => prop.UserId == UserId)
                             .Include(prop => prop.Test)
                                .ThenInclude(prop => prop.Setting)
                             .OrderByDescending(prop => prop.SubmitDate)
                             .ToListAsync();

            examresult.Tests = _mapper.Map<List<SubmittedTestModel>>(submittedTests);

            return examresult;
        }
    }
}
