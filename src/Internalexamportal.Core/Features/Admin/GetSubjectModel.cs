using AutoMapper;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features
{
    public class GetSubjectModel : IRequest<ICollection<GetSubjectResult>>
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }

    }
    public class GetSubjectResult
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
    }

    public class GetDataHandler : IRequestHandler<GetSubjectModel, ICollection<GetSubjectResult>>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IClientManagerService _clientManager;
        public GetDataHandler(InternalExamportalContext dbContext,
            IMapper mapper,
            IClientManagerService clientManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _clientManager = clientManager;
        }

        public async Task<ICollection<GetSubjectResult>> Handle(GetSubjectModel request, CancellationToken cancellationToken)
        {
            var client = await _clientManager.GetClientId();

            var subjects = await _dbContext.Subject
                .Where(prop => client.Item2 || client.Item1 == prop.ClientId)
                .ToListAsync(cancellationToken);

            var mapped = _mapper.Map<List<GetSubjectResult>>(subjects);
            return mapped; // List<T> implements ICollection<T> naturally
        }

    }

}
