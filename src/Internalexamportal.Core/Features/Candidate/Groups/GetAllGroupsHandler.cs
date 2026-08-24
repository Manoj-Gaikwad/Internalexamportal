using AutoMapper;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Threading;
using System;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Commons.Utils;
using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Internalexamportal.Core.Features.Candidate
{

    //Model
    public class GetAllGroupsModel : IRequest<GetAllGroupsResult>
    {

    }

    public class GetAllGroupsResult
    {
        public List<Group> Groups { get; set; }
    }

    //Handler
    public class GetAllGroupsHandler : IRequestHandler<GetAllGroupsModel, GetAllGroupsResult>
    {
        private readonly IMapper _mapper;
        private readonly InternalExamportalContext _dbContext;
        private readonly IClientManagerService _clientManager;

        public GetAllGroupsHandler(
            IMapper mapper,
            InternalExamportalContext dbContext,
            IClientManagerService clientManager
        )
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _clientManager = clientManager;
        }

        public async Task<GetAllGroupsResult> Handle(GetAllGroupsModel request, CancellationToken token)
        {
            var result = new GetAllGroupsResult();

            var client = await _clientManager.GetClientId();

            result.Groups =  await _dbContext.Group
                                                 .Where(
                                                        prop => (client.Item2 || client.Item1 == prop.ClientId) 
                                                        && prop.IsDeleted==false)
                                                 .ToListAsync();

            return result;
        }
    }
}
