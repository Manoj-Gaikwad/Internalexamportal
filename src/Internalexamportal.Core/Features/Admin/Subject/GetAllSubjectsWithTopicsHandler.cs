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
    public class GetAllSubjectsWithTopicsModel : IRequest<ICollection<GetAllSubjectsWithTopicsResult>>
    {
    }

    public class GetAllSubjectsWithTopicsResult
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public List<TopicModel> SubjectTopic { get; set; }
    }

    public class TopicModel
    {
        public int Id { get; set; }
        public string Topic { get; set; }
    }

    public class GetAllSubjectsWithTopicsHandler : IRequestHandler<GetAllSubjectsWithTopicsModel, ICollection<GetAllSubjectsWithTopicsResult>>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IClientManagerService _clientManager;
        public GetAllSubjectsWithTopicsHandler(InternalExamportalContext dbContext,
            IMapper mapper,
            IClientManagerService clientManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _clientManager = clientManager;
        }

        public async Task<ICollection<GetAllSubjectsWithTopicsResult>> Handle(
     GetAllSubjectsWithTopicsModel request,
     CancellationToken cancellationToken)
        {
            var client = await _clientManager.GetClientId();

            var subjects = await _dbContext.Subject
                .Include(prop => prop.SubjectTopic)
                .Where(prop => client.Item2 || client.Item1 == prop.ClientId)
                .ToListAsync(cancellationToken);

            var mapped = _mapper.Map<List<GetAllSubjectsWithTopicsResult>>(subjects);
            return mapped; // List<T> implements ICollection<T>
        }

    }

}
