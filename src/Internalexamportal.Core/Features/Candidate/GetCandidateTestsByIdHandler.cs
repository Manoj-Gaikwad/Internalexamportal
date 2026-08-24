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
    public class GetCandidateTestsByIdRequestModel : IRequest<GetCandidateTestsByIdResponseModel>
    {
        public string CandidateId { get; set; }
    }

    public class GetCandidateTestsByIdResponseModel
    {
        public List<SubmittedTest> Tests { get; set; }
    }

    public class GetCandidateTestsByIdHandler : IRequestHandler<GetCandidateTestsByIdRequestModel, GetCandidateTestsByIdResponseModel>
    {

        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPrincipal _principle;

        public GetCandidateTestsByIdHandler(IMapper mapper, InternalExamportalContext dbContext, IPrincipal principle)
        {
            _dbContext = dbContext;
            _principle = principle;
            _mapper = mapper;
        }

        public async Task<GetCandidateTestsByIdResponseModel> Handle(GetCandidateTestsByIdRequestModel request, CancellationToken cancellationToken)
        {
            var examresult = new GetCandidateTestsByIdResponseModel();

            examresult.Tests = await _dbContext.SubmittedTest
                             .Where(prop => prop.UserId == request.CandidateId)
                             .Include(prop => prop.Test)
                             .OrderByDescending(prop => prop.SubmitDate)
                             .ToListAsync();

            return examresult;
        }
    }
}
