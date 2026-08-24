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
    public class GetExamReportRequestModel : IRequest<GetExamReportResponseModel>
    {
        public int Id { get; set; }
    }

    public class GetExamReportResponseModel
    {
        public int AttemptedQuestion { get; set; }
        public int SkippedQuestion { get; set; }
        public int ReviewedQuestion { get; set; }
        public int CorrectQuestion { get; set; }
        public int InCorrectQuestion { get; set; }
        public int MaximumMark { get; set; }
        public int RightMark { get; set; }
        public int NegativeMark { get; set; }
        public int TotalMark { get; set; }
        public string TotalTime { get; set; }
        public string TimeTaken { get; set; }
        public string Subjects { get; set; }
        public string UserId { get; set; }
        public int TestId { get; set; }
    }

    public class GetExamReportHandler : IRequestHandler<GetExamReportRequestModel, GetExamReportResponseModel>
    {

        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPrincipal _principle;

        public GetExamReportHandler(IMapper mapper, InternalExamportalContext dbContext, IPrincipal principle)
        {
            _dbContext = dbContext;
            _principle = principle;
            _mapper = mapper;
        }

        public async Task<GetExamReportResponseModel> Handle(GetExamReportRequestModel request, CancellationToken cancellationToken)
        {
            var examresult = new GetExamReportResponseModel();

            var UserId = _principle.Identity.GetUserId();

            var result = _dbContext.SubmittedTest
                            .Include(prop => prop.Test)
                            .Where(prop => prop.Id == request.Id && prop.UserId == UserId)
                            .OrderByDescending(prop => prop.Id)
                            .FirstOrDefault();

            examresult = _mapper.Map<GetExamReportResponseModel>(result);

            var subjects = await _dbContext.TestQuestion
                 .Include(prop => prop.Question)
                 .ThenInclude(prop => prop.Subject)
                 .Where(prop => prop.TestId == result.TestId)
                 .Select(prop => prop.Question.Subject.SubjectName)
                 .Distinct()
                 .ToListAsync();

            examresult.Subjects = string.Join(", ", subjects);

            return examresult;
        }
    }
}
