using Internalexamportal.Core.Features.GenerateJwtToken;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.ExamResult
{
    public class ExamResultModel : IRequest<CandidateExamResult>
    {
        public int QuestionId { get; set; }
        public int QuestionOptionId { get; set; }

    }
    public class CandidateExamResult
    {
        public TokenResult TokenResult { get; set; }
        public Boolean Suceeded { get; set; }
   
    }
    public class ExamResultHandler : IRequestHandler<ExamResultModel,CandidateExamResult>
    {
        public ExamResultHandler(){
        }

        public async Task<CandidateExamResult> Handle(ExamResultModel resultrequest, CancellationToken cancellationToken)
        {
            var CandidateExamResult = new CandidateExamResult();
            return CandidateExamResult;
            //throw new NotImplementedException();
        }
    }
}
