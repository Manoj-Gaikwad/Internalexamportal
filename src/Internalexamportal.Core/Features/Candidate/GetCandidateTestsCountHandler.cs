using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.DataAccessLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Candidate
{
    public class GetCandidateTestsCountRequestModel : IRequest<GetCandidateTestsCountResponseModel>
    {
    }

    public class GetCandidateTestsCountResponseModel
    {
        public int Count { get; set; }
    }

    public class GetCandidateTestsCountHandler : IRequestHandler<GetCandidateTestsCountRequestModel, GetCandidateTestsCountResponseModel>
    {

        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPrincipal _principle;

        public GetCandidateTestsCountHandler(IMapper mapper, InternalExamportalContext dbContext, IPrincipal principle)
        {
            _dbContext = dbContext;
            _principle = principle;
            _mapper = mapper;
        }

        public async Task<GetCandidateTestsCountResponseModel> Handle(GetCandidateTestsCountRequestModel request, CancellationToken cancellationToken)
        {
            var examresult = new GetCandidateTestsCountResponseModel();

            var UserId = _principle.Identity.GetUserId();

           examresult.Count = await _dbContext.SubmittedTest
                            .Where(prop => prop.UserId == UserId)
                            .CountAsync();

            return examresult;
        }
    }
}
