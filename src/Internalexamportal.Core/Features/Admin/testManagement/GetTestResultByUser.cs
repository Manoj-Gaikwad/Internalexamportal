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

namespace Internalexamportal.Core.Features.Admin.testManagement
{
    public class GetTestResultByIdRequestModel : IRequest<GetTestResultByIdResponseModel>
    {
        public int TestResultId { get; set; }
    }
    public class GetTestResultByIdResponseModel
    {
        public int Id { get; set; }
        public DateTime SubmitDate { get; set; }
        public int AttemptedQuestion { get; set; }
        public int SkippedQuestion { get; set; }
        public int ReviewedQuestion { get; set; }
        public int CorrectQuestion { get; set; }
        public int InCorrectQuestion { get; set; }
        public int MaximumMark { get; set; }
        public int RightMark { get; set; }
        public int NegativeMark { get; set; }
        public int TotalMark { get; set; }
        public float Percentage { get; set; }
        public string UserName { get; set; }
        public string TestName { get; set; }
        public string RollNumber { get; set; }
    }

    public class GetTestResultByIdHandler : IRequestHandler<GetTestResultByIdRequestModel, GetTestResultByIdResponseModel>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetTestResultByIdHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetTestResultByIdResponseModel> Handle(GetTestResultByIdRequestModel request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.SubmittedTest
                                    .Where(prop => prop.Id == request.TestResultId)
                                    .Include(prop => prop.User)
                                    .Include(prop => prop.Test)
                                    .FirstOrDefaultAsync();

            return _mapper.Map<GetTestResultByIdResponseModel>(result);
        }


    }
}
