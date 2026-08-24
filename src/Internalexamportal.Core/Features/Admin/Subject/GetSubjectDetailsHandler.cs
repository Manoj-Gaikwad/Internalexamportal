using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features
{
    public class GetSubjectDetailsModel : IRequest<GetSubjectDetailsResult>
    {
        public int Id { get; set; }

    }
    public class GetSubjectDetailsResult
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
    }

    public class GetSubjectDetailsHandler : IRequestHandler<GetSubjectDetailsModel, GetSubjectDetailsResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetSubjectDetailsHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetSubjectDetailsResult> Handle(GetSubjectDetailsModel request, CancellationToken cancellationToken)
        {
            var subject = await _dbContext.Subject.FindAsync(request.Id);

            return _mapper.Map<GetSubjectDetailsResult>(subject);
        }
    }

}
