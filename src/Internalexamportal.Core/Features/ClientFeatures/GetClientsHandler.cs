using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.ClientFeatures
{
    public class GetClientsModel : IRequest<GetClientsResult>
    {
    }
    public class GetClientsResult
    {
        public List<GetClientModel> Clients { get; set; }
        public int TotalCount { get; set; }
    }

    public class GetClientModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GetClientsHandler : IRequestHandler<GetClientsModel, GetClientsResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public GetClientsHandler(IMapper mapper, InternalExamportalContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetClientsResult> Handle(GetClientsModel request, CancellationToken cancellationToken)
        {
            var result = new GetClientsResult();

           var clients =await _dbContext.Client.ToListAsync();

            result.Clients = _mapper.Map<List<GetClientModel>>(clients);

            result.TotalCount = clients.Count();

            return result;
        }

    }

}
